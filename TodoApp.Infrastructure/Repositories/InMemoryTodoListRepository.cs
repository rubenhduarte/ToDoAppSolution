using TodoApp.Domain.Interfaces;

namespace TodoApp.Infrastructure.Repositories;

public class InMemoryTodoListRepository : ITodoListRepository
{
    private int _currentId = 0;
    private readonly List<string> _categories = new() { "Work", "Personal", "Hobby" };

    public int GetNextId()
    {
        _currentId++;
        return _currentId;
    }

    public List<string> GetAllCategories() => _categories;
}