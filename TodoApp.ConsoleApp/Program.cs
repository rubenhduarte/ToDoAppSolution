
using TodoApp.Application.Services;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Interfaces;
using TodoApp.Infrastructure.Repositories;

ITodoListRepository repository = new InMemoryTodoListRepository();
ITodoList todoList = new TodoList(repository);
TodoListService service = new TodoListService(todoList, repository);

// Ejemplo de uso:
int id = service.CreateTodoItem("Complete Project Report",
                                "Finish the final report for the project", "Work");
service.AddProgression(id, new DateTime(2025, 03, 18), 30);
service.AddProgression(id, new DateTime(2025, 03, 19), 50);
service.AddProgression(id, new DateTime(2025, 03, 20), 20);

service.PrintTodoItems();

Console.WriteLine("Presiona ENTER para finalizar...");
Console.ReadLine();

