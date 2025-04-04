namespace TodoApp.Application.TodoItems.CreateCommand;

public class CreateTodoItemCommand
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
}