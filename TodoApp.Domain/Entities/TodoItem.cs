namespace TodoApp.Domain.Entities;
public class TodoItem 
{
    public int Id { get; private set; }
    public string Title { get; private set;}
    public string Description { get; private set;}
    public string Category { get; private set;}
    
    public List<Progression> Progressions { get; private set; } = new();
    public bool IsCompleted => TotalProgress() == 100;
    public decimal TotalProgress() => Progressions.Sum(p => p.Percent);
    public TodoItem(string title,
                    string description,
                    string category) {
        Title = title;
        Description = description;
        Category = category;
        Progressions = new List<Progression>();
    }

    private TodoItem() { } 


    public void UpdateDescription(string description)
    {
        Description = description;
    }
    public void AddProgression(Progression progression)
    {
        Progressions.Add(progression);
    }

}