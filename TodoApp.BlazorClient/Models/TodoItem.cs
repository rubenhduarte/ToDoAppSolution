namespace TodoApp.BlazorClient.Models;

public class TodoItem
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public string Category { get; set; } = "";
    public List<Progression> Progressions { get; set; } = new();
    public bool IsCompleted { get; set; }

    public decimal NewPercent { get; set; } = 1;
}