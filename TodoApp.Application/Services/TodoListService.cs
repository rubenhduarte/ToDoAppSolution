
using FluentValidation;
using FluentValidation.Results;
using TodoApp.Application.Progression.Commands;
using TodoApp.Application.TodoItems.CreateCommand;
using TodoApp.Application.TodoItems.UpdateCommand;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Interfaces;

namespace TodoApp.Application.Services;
public class TodoListService
{
    private readonly ITodoList _todoList;
    private readonly ITodoListRepository _repository;

    public TodoListService(ITodoList todoList, ITodoListRepository repository)
    {
        _todoList = todoList;
        _repository = repository;
    }

    public void CreateTodoItem(string title, string description, string category)
    {
        var command = new CreateTodoItemCommand
        {
            Title = title,
            Description = description,
            Category = category
        };

        var validator = new CreateTodoItemCommandValidator(_repository);
        ValidationResult result = validator.Validate(command);
        if (!result.IsValid)
            throw new ValidationException(result.Errors);

        _todoList.AddItem(title, description, category);
    }

    public void UpdateTodoItem(int id, string newDescription)
    {
        var item = _todoList.GetItemById(id);
        if (item == null)
            throw new ArgumentException("El item no existe.");

        var command = new UpdateTodoItemCommand
        {
            TodoItemId = id,
            NewDescription = newDescription
        };

        var validator = new UpdateTodoItemCommandValidator(item);
        ValidationResult result = validator.Validate(command);
        if (!result.IsValid)
            throw new ValidationException(result.Errors);

        _todoList.UpdateItem(id, newDescription);
    }

    public void RemoveTodoItem(int id)
    {
        var item = _todoList.GetItemById(id);
        if (item == null)
            throw new ArgumentException("El item no existe.");

        if (item.TotalProgress() > 50)
            throw new InvalidOperationException("No se puede eliminar un item con más del 50% completado.");

        _todoList.RemoveItem(id);
    }

    public void AddProgression(int id, DateTime dateTime, decimal percent)
    {
        var item = _todoList.GetItemById(id);
        if (item == null)
            throw new ArgumentException("El item no existe.");

        var command = new RegisterProgressionCommand
        {
            TodoItemId = id,
            DateTime = dateTime,
            Percent = percent
        };

        var validator = new RegisterProgressionCommandValidator(item);
        ValidationResult result = validator.Validate(command);
        if (!result.IsValid)
            throw new ValidationException(result.Errors);

        _todoList.RegisterProgression(id, dateTime, percent);
    }

    public List<TodoItem> GetAllItems()
    {
        return _todoList.GetAllItems();
    }

    public TodoItem GetItemById(int id)
    {
        return _todoList.GetItemById(id);
    }

    public void PrintTodoItems()
    {
        _todoList.PrintItems();
    }
}