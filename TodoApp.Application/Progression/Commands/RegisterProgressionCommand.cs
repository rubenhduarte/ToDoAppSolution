namespace TodoApp.Application.Progression.Commands; 

public class RegisterProgressionCommand
{
    public int TodoItemId { get; set; }
    public DateTime DateTime { get; set; }
    public decimal Percent { get; set; }
}