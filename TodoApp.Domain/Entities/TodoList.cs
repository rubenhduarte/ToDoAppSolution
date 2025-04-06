using TodoApp.Domain.Interfaces;

namespace TodoApp.Domain.Entities;
public class TodoList : ITodoList
{
    // Se expone la lista para facilitar el acceso desde la capa de aplicación (aunque se puede encapsular mejor)
    public List<TodoItem> Items { get; private set; }
    private readonly ITodoListRepository _repository;

    public TodoList(ITodoListRepository repository)
    {
        _repository = repository;
        Items = new List<TodoItem>();
    }

    public void AddItem(string title, 
                        string description, 
                        string category)
    {
        if (!_repository.GetAllCategories().Contains(category))
            throw new ArgumentException("La categoría no es válida.");

        var newItem = new TodoItem(title, description, category);
        Items.Add(newItem);
    }

    public void UpdateItem(int id, 
                           string description)
    {
        var item = Items.FirstOrDefault(x => x.Id == id);
        if (item == null)
            throw new ArgumentException("El item no existe.");

        if (item.TotalProgress() > 50)
            throw new InvalidOperationException("No se puede actualizar un item con más del 50% completado.");

        item.UpdateDescription(description);
    }

    public void RemoveItem(int id)
    {
        var item = Items.FirstOrDefault(x => x.Id == id);
        if (item == null)
            throw new ArgumentException("El item no existe.");

        if (item.TotalProgress() > 50)
            throw new InvalidOperationException("No se puede remover un item con más del 50% completado.");

        Items.Remove(item);
    }

    public void RegisterProgression(int id, 
                                    DateTime dateTime, 
                                    decimal percent)
    {
        var item = Items.FirstOrDefault(x => x.Id == id);
        if (item == null)
            throw new ArgumentException("El item no existe.");

        if (percent <= 0 || percent >= 100)
            throw new ArgumentException("El porcentaje debe ser mayor a 0 y menor a 100.");

        if (item.Progressions.Any())
        {
            var lastDate = item.Progressions.Max(p => p.Date);
            if (dateTime <= lastDate)
                throw new ArgumentException("La fecha de la nueva progresión debe ser mayor que la última progresión.");
        }

        if (item.TotalProgress() + percent > 100)
            throw new InvalidOperationException("La suma total de progresiones no puede exceder el 100%.");

        item.Progressions.Add(new Progression(dateTime, percent));
    }

    public void PrintItems()
    {
        var sortedItems = Items.OrderBy(x => x.Id);
        foreach (var item in sortedItems)
        {
            Console.WriteLine($"{item.Id}) {item.Title} - {item.Description} ({item.Category}) Completed:{item.IsCompleted}");
            decimal cumulative = 0;
            foreach (var prog in item.Progressions.OrderBy(p => p.Date))
            {
                cumulative += prog.Percent;
                int progressBars = (int)(cumulative / 2);
                string bar = new string('O', progressBars);
                Console.WriteLine($"{prog.Date} - {cumulative}% |{bar}|");
            }
        }
    }
    public TodoItem GetItemById(int id)
    {
        return Items.FirstOrDefault(x => x.Id == id);
    }
    public List<TodoItem> GetAllItems()
    {
        return Items;
    }
}