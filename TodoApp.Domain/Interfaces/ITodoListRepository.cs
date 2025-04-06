namespace TodoApp.Domain.Interfaces;

public interface ITodoListRepository
{
    List<string> GetAllCategories();
}