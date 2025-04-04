
using TodoApp.Application.Progression.Commands;
using TodoApp.Application.TodoItems.CreateCommand;
using TodoApp.Application.TodoItems.UpdateCommand;
using TodoApp.Domain.Entities;

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
            // Se obtiene el TodoItem (para este ejemplo, asumimos que el _todoList interno lo tiene expuesto o lo podemos obtener de alguna forma)
            // Aquí se asume que _todoList tiene un método interno GetItemById, o se utiliza algún mecanismo para obtenerlo.
            // Para fines de este ejemplo, se recurre a la lógica interna (idealmente, encapsularías este proceso)
            var todoItem = (_todoList as dynamic).Items.FirstOrDefault(item => item.Id == id) as TodoItem;
            if (todoItem == null)
                throw new ArgumentException("El item no existe.");

            var command = new RegisterProgressionCommand { TodoItemId = id, DateTime = dateTime, Percent = percent };
            var validator = new RegisterProgressionCommandValidator(todoItem);
            var result = validator.Validate(command);
            if (!result.IsValid)
                throw new ValidationException(result.Errors);
            
            _todoList.RegisterProgression(id, dateTime, percent);
        }
        
        // Métodos para UpdateTodoItem y RemoveTodoItem aplicarían validaciones similares.
        public void UpdateTodoItem(int id, string description)
        {
            var todoItem = (_todoList as dynamic).Items.FirstOrDefault(item => item.Id == id) as TodoItem;
            if (todoItem == null)
                throw new ArgumentException("El item no existe.");
            
            var command = new UpdateTodoItemCommand { TodoItemId = id, NewDescription = description };
            var validator = new UpdateTodoItemCommandValidator(todoItem);
            var result = validator.Validate(command);
            if (!result.IsValid)
                throw new ValidationException(result.Errors);
            
            _todoList.UpdateItem(id, description);
        }
        
        public void RemoveTodoItem(int id)
        {
            var todoItem = (_todoList as dynamic).Items.FirstOrDefault(item => item.Id == id) as TodoItem;
            if (todoItem == null)
                throw new ArgumentException("El item no existe.");
            
            // Validar que no se exceda el 50%
            if (todoItem.TotalProgress() > 50)
                throw new ValidationException("No se puede remover un item con más del 50% completado.");
            
            _todoList.RemoveItem(id);
        }
        
        public void PrintTodoItems() => _todoList.PrintItems();
    }