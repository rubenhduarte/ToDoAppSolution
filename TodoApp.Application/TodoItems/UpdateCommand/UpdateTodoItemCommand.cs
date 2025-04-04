namespace TodoApp.Application.TodoItems.UpdateCommand;

public class UpdateTodoItemCommand
{
    public int TodoItemId { get; set; }
    public string NewDescription { get; set; }
}