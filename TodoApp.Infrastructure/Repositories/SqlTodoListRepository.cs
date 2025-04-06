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

    public List<string> GetAllCategories()
    {
        // Para simplificar, se devuelve una lista fija.
        // También podrías tener una tabla de Categorías en la base de datos.
        return new List<string> { "Work", "Personal", "Hobby" };
    }
}