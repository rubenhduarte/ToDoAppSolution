namespace TodoApp.Domain;

public class TodoItem
{
    public int Id { get; }
    public string Title { get; }
    public string Description { get; private set; }
    public string Category { get; }
    public List<Progression> Progressions { get; }
        
    public TodoItem(int id, 
                    string title, 
                    string description, 
                    string category)
    {
        Id = id;
        Title = title;
        Description = description;
        Category = category;
        Progressions = new List<Progression>();
    }
        
    // Se considera completado si la suma de progresiones es 100%
    public bool IsCompleted => TotalProgress() == 100;
        
    public decimal TotalProgress() => Progressions.Sum(p => p.Percent);
        
    public void UpdateDescription(string description) => Description = description;
}