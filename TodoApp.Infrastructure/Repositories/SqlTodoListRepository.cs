using Microsoft.EntityFrameworkCore;
using TodoApp.Domain.Entities;
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

    public int Add(TodoItem item)
    {
        _context.TodoItems.Add(item);
        _context.SaveChanges();
        return item.Id;
    }

    public void Update(TodoItem item)
    {
        _context.TodoItems.Update(item);
        _context.SaveChanges();
    }

    public void Remove(TodoItem item)
    {
        _context.TodoItems.Remove(item);
        _context.SaveChanges();
    }

    public List<TodoItem> GetAll()
    {
        return _context.TodoItems
            .Include(t => t.Progressions)
            .OrderBy(t => t.Id)
            .ToList();
    }

    public TodoItem? GetById(int id)
    {
        return _context.TodoItems
            .Include(t => t.Progressions)
            .FirstOrDefault(t => t.Id == id);
    }


    public List<string> GetAllCategories()
    {
        // Para simplificar, se devuelve una lista fija.
        // También podrías tener una tabla de Categorías en la base de datos.
        return new List<string> { "Work", "Personal", "Hobby" };
    }

}