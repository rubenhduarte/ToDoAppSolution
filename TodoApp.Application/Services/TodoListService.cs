
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
        
        public int CreateTodoItem(string title, string description, string category)
        {
            // Validar el comando de creación
            var createCmd = new CreateTodoItemCommand { Title = title, Description = description, Category = category };
            var createValidator = new CreateTodoItemCommandValidator(_repository);
            ValidationResult result = createValidator.Validate(createCmd);
            if (!result.IsValid)
                throw new ValidationException(result.Errors);

            int id = _repository.GetNextId();
            _todoList.AddItem(id, title, description, category);
            return id;
        }
        
        public void AddProgression(int id, DateTime dateTime, decimal percent)
        {
            var todoItem = _todoList.GetItemById(id);
            if (todoItem == null)
                throw new ArgumentException("El item no existe.");

            var command = new RegisterProgressionCommand { TodoItemId = id, 
                                                           DateTime = dateTime, 
                                                           Percent = percent };

            var validator = new RegisterProgressionCommandValidator(todoItem);
            var result = validator.Validate(command);
            if (!result.IsValid)
                throw new ValidationException(result.Errors);

            _todoList.RegisterProgression(id, dateTime, percent);

        }
        
        // Métodos para UpdateTodoItem y RemoveTodoItem aplicarían validaciones similares.
        public void UpdateTodoItem(int id, string newDescription)
        {

            var todoItem = _todoList.GetItemById(id);
            if (todoItem == null)
                throw new ArgumentException("El item no existe.");

            var command = new UpdateTodoItemCommand
            {
                TodoItemId = id,
                NewDescription = newDescription
            };

            var validator = new UpdateTodoItemCommandValidator(todoItem);
            ValidationResult result = validator.Validate(command);
            if (!result.IsValid)
                throw new ValidationException(result.Errors);

            _todoList.UpdateItem(id, newDescription);
        }
        
        public void RemoveTodoItem(int id)
        {
            var todoItem = _todoList.GetItemById(id);
            if (todoItem == null)
                throw new ArgumentException("El item no existe.");

            if (todoItem.TotalProgress() > 50)
                throw new ValidationException("No se puede remover un item con más del 50% completado.");

            _todoList.RemoveItem(id);
        }
        
        public void PrintTodoItems() => _todoList.PrintItems();
    }