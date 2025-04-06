using TodoApp.Domain.Interfaces;
using TodoApp.Infrastructure.Data;

namespace TodoApp.Infrastructure.Repositories;

public class SqlTodoListRepository : ITodoListRepository
{
    private readonly TodoDbContext _context;

    public SqlTodoListRepository(TodoDbContext context)
    {
        _context = context;
    }

    public int GetNextId()
    {
        // Se obtiene el máximo Id registrado en la tabla TodoItems y se suma 1.
        // Nota: En un entorno real, se usaría una columna Identity.
        int maxId = _context.TodoItems.Any() ? _context.TodoItems.Max(t => t.Id) : 0;
        return maxId + 1;
    }

    public List<string> GetAllCategories()
    {
        // Para simplificar, se devuelve una lista fija.
        // También podrías tener una tabla de Categorías en la base de datos.
        return new List<string> { "Work", "Personal", "Hobby" };
    }
}