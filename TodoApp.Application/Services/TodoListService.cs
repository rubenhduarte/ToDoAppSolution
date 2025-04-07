
using FluentValidation;
using FluentValidation.Results;
using TodoApp.Application.Progressions.Commands;
using TodoApp.Application.TodoItems.CreateCommand;
using TodoApp.Application.TodoItems.UpdateCommand;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Interfaces;

namespace TodoApp.Application.Services;
public class TodoListService
{
    private readonly ITodoListRepository _repository;

    public TodoListService(ITodoListRepository repository)
    {
        _repository = repository;
    }

    public int CreateTodoItem(string title, string description, string category)
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

        var todoItem = new TodoItem(title, description, category);
        _repository.Add(todoItem);
        return todoItem.Id;
    }

    public void UpdateTodoItem(int id, string newDescription)
    {
        var item = _repository.GetById(id);
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

        item.UpdateDescription(newDescription);
        _repository.Update(item);
    }

    public void RemoveTodoItem(int id)
    {
        var item = _repository.GetById(id);
        if (item == null)
            throw new ArgumentException("El item no existe.");

        if (item.TotalProgress() > 50)
            throw new InvalidOperationException("No se puede eliminar un item con más del 50% completado.");

        _repository.Remove(item);
    }

    public void AddProgression(int id, DateTime dateTime, decimal percent)
    {
        var item = _repository.GetById(id);
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

        var progression = new Progression(dateTime, percent);
        item.AddProgression(progression);
        _repository.Update(item);
    }

    public List<TodoItem> GetAllItems()
    {
        return _repository.GetAll();
    }

    public TodoItem? GetItemById(int id)
    {
        return _repository.GetById(id);
    }

    public void PrintTodoItems()
    {
        var items = _repository.GetAll();
        foreach (var item in items)
        {
            Console.WriteLine($"{item.Id}) {item.Title} - {item.Description} ({item.Category}) Completed: {item.IsCompleted}");
            decimal cumulative = 0;
            foreach (var prog in item.Progressions.OrderBy(p => p.Date))
            {
                cumulative += prog.Percent;
                int progressBars = (int)(cumulative / 2);
                string bar = new string('O', progressBars);
                Console.WriteLine($"{prog.Date:g} - {cumulative}% |{bar}|");
            }
        }
    }
}