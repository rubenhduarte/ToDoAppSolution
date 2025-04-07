using TodoApp.Domain.Entities;

namespace TodoApp.Domain.Interfaces;

public interface ITodoListRepository
{
    int Add(TodoItem item);
    void Update(TodoItem item);
    void Remove(TodoItem item);

    List<TodoItem> GetAll();
    TodoItem? GetById(int id);            
    List<string> GetAllCategories();  
    
}