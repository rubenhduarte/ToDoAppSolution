
using Microsoft.EntityFrameworkCore;
using TodoApp.Application.Services;
using TodoApp.Infrastructure.Data;
using TodoApp.Infrastructure.Repositories;

internal class Program {
    private static void Main(string[] args) {
        var options = new DbContextOptionsBuilder<TodoDbContext>()
    .UseInMemoryDatabase("ConsoleTodoDb")
    .Options;

        var context = new TodoDbContext(options);
        var repository = new SqlTodoListRepository(context);
        var service = new TodoListService(repository);
        // Ejemplo de uso:
        int id = service.CreateTodoItem("Complete Project Report",
                                        "Finish the final report for the project","Work");
        service.AddProgression(id,new DateTime(2025,03,18),30);
        service.AddProgression(id,new DateTime(2025,03,19),50);
        service.AddProgression(id,new DateTime(2025,03,20),20);

        service.PrintTodoItems();

        Console.WriteLine("Presiona ENTER para finalizar...");
        Console.ReadLine();
    }
}