using Microsoft.EntityFrameworkCore;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Interfaces;
using TodoApp.Infrastructure.Data;

namespace TodoApp.Infrastructure.Repositories;

public class TodoListEF : ITodoList
{
    private readonly TodoDbContext _context;
    private readonly ITodoListRepository _repository;

    public TodoListEF(TodoDbContext context, ITodoListRepository repository)
    {
        _context = context;
        _repository = repository;
    }

    // Agrega un nuevo TodoItem; el Id se genera automáticamente.
    public void AddItem(string title, 
                        string description, 
                        string category)
    {
        var todoItem = new TodoItem(title, description, category);
        _context.TodoItems.Add(todoItem);
        _context.SaveChanges();
    }

    public void UpdateItem(int id, 
                           string description)
    {
        var item = GetItemById(id);
        if (item == null) throw new ArgumentException("No encontrado.");
        if (item.TotalProgress() > 50) throw new 
                 InvalidOperationException("No se puede actualizar.");

        item.UpdateDescription(description);
        _context.SaveChanges();
    }

    public void RemoveItem(int id)
    {
        var item = GetItemById(id);
        if (item == null) throw new ArgumentException("No encontrado.");
        if (item.TotalProgress() > 50) throw 
                    new InvalidOperationException("No se puede eliminar.");

        _context.TodoItems.Remove(item);
        _context.SaveChanges();
    }

    public void RegisterProgression(int id, DateTime dateTime, decimal percent)
    {
        var item = GetItemById(id);
        if (item == null) throw new ArgumentException("No encontrado.");

        if (item.Progressions.Any() && dateTime <= item.Progressions.Max(p => p.Date))
            throw new ArgumentException("Fecha inválida.");

        if (item.TotalProgress() + percent > 100)
            throw new InvalidOperationException("Supera 100%.");

        item.AddProgression(new Progression(dateTime, percent));
        _context.SaveChanges();
    }

    public TodoItem GetItemById(int id)
    {
        return _context.TodoItems.Include(p => p.Progressions).
            FirstOrDefault(x => x.Id == id);
    }

    public void PrintItems()
    {

        var items = GetAllItems();
        foreach (var item in items)
        {
            Console.WriteLine($"{item.Id}) {item.Title} - " +
                    $"{item.Description} ({item.Category}) Completed: {item.IsCompleted}");
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

    // Implementación de GetItemById para obtener un TodoItem específico.

    public List<TodoItem> GetAllItems()
    {
        return _context.TodoItems.Include("Progressions").ToList();
    }
}