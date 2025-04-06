using Microsoft.EntityFrameworkCore;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Interfaces;
using TodoApp.Infrastructure.Data;

namespace TodoApp.Infrastructure.Repositories;

public class TodoListEF : ITodoList
{
    private readonly TodoDbContext _context;

    public TodoListEF(TodoDbContext context)
    {
        _context = context;
    }

    // Agrega un nuevo TodoItem; el Id se genera automáticamente.
    public void AddItem(string title, string description, string category)
    {
        var todoItem = new TodoItem(title, description, category);
        _context.TodoItems.Add(todoItem);
        _context.SaveChanges();
    }

    public void UpdateItem(int id, string description)
    {
        var todoItem = _context.TodoItems
            .Include(t => t.Progressions)
            .FirstOrDefault(t => t.Id == id);

        if (todoItem == null)
            throw new ArgumentException("El item no existe.");

        if (todoItem.TotalProgress() > 50)
            throw new InvalidOperationException("No se puede actualizar un item con más del 50% completado.");

        todoItem.UpdateDescription(description);
        _context.SaveChanges();
    }

    public void RemoveItem(int id)
    {
        var todoItem = _context.TodoItems
            .Include(t => t.Progressions)
            .FirstOrDefault(t => t.Id == id);

        if (todoItem == null)
            throw new ArgumentException("El item no existe.");

        if (todoItem.TotalProgress() > 50)
            throw new InvalidOperationException("No se puede remover un item con más del 50% completado.");

        _context.TodoItems.Remove(todoItem);
        _context.SaveChanges();
    }

    public void RegisterProgression(int id, DateTime dateTime, decimal percent)
    {
        var todoItem = _context.TodoItems
            .Include(t => t.Progressions)
            .FirstOrDefault(t => t.Id == id);

        if (todoItem == null)
            throw new ArgumentException("El item no existe.");

        if (percent <= 0 || percent >= 100)
            throw new ArgumentException("El porcentaje debe ser mayor a 0 y menor a 100.");

        if (todoItem.Progressions.Any())
        {
            var lastDate = todoItem.Progressions.Max(p => p.Date);
            if (dateTime <= lastDate)
                throw new ArgumentException("La fecha de la nueva progresión debe ser mayor que la última progresión.");
        }

        if (todoItem.TotalProgress() + percent > 100)
            throw new InvalidOperationException("La suma total de progresiones no puede exceder el 100%.");

        var progression = new Progression(dateTime, percent);
        todoItem.AddProgression(progression);
        _context.SaveChanges();
    }

    public void PrintItems()
    {
        var items = _context.TodoItems
            .Include(t => t.Progressions)
            .OrderBy(t => t.Id)
            .ToList();

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
    public TodoItem GetItemById(int id)
    {
        return _context.TodoItems
            .Include(t => t.Progressions)
            .FirstOrDefault(t => t.Id == id);
    }
}