namespace TodoApp.BlazorClient.Models;

public class Progression
{
    public DateTime Date { get; set; }
    public decimal Percent { get; set; }
}

public class TodoItem
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public List<Progression> Progressions { get; set; }
    public bool IsCompleted { get; set; }
}