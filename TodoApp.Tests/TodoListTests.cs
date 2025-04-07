using FluentValidation;
using TodoApp.Application.Services;
using TodoApp.Infrastructure.Data;
using TodoApp.Infrastructure.Repositories;
using TodoApp.Tests.ContextFactory;

namespace TodoApp.Tests;

public class TodoListTests : IDisposable
{    
    private readonly TodoDbContext _context;
    private readonly SqlTodoListRepository _repository;
    private readonly TodoListService _service;

    public TodoListTests()
    {
        _context = TestDbContextFactory.CreateInMemoryContext();
        _repository = new SqlTodoListRepository(_context);
        _service = new TodoListService(_repository);
    }

    [Fact]
    public void CreateTodoItem_HappyPath()
    {
        string title = "Complete Project Report";
        string description = "Finish the final report for the project";
        string category = "Work";

        int id = _service.CreateTodoItem(title, description, category);

        var item = _repository.GetById(id);
        Assert.NotNull(item);
        Assert.Equal(title, item.Title);
        Assert.Equal(description, item.Description);
        Assert.Equal(category, item.Category);
    }

    [Fact]
    public void RegisterProgression_HappyPath()
    {
        int id = _service.CreateTodoItem("Test", "Test Desc", "Work");
        DateTime date1 = new DateTime(2025, 03, 18);
        DateTime date2 = new DateTime(2025, 03, 19);

        _service.AddProgression(id, date1, 30);
        _service.AddProgression(id, date2, 50);

        var item = _repository.GetById(id);
        Assert.Equal(2, item.Progressions.Count);
        Assert.Equal(30, item.Progressions[0].Percent);
        Assert.Equal(50, item.Progressions[1].Percent);
        Assert.Equal(80, item.TotalProgress());
    }

    [Fact]
    public void RegisterProgression_InvalidDate_ShouldThrow()
    {
        int id = _service.CreateTodoItem("Test", "Test Desc", "Work");
        DateTime date1 = new DateTime(2025, 03, 18);
        DateTime invalidDate = new DateTime(2025, 03, 17);

        _service.AddProgression(id, date1, 30);

        Assert.Throws<ValidationException>(() => _service.AddProgression(id, invalidDate, 20));
    }

    [Fact]
    public void RegisterProgression_ExceedTotalProgress_ShouldThrow()
    {
        int id = _service.CreateTodoItem("Test", "Test Desc", "Work");
        DateTime date1 = new DateTime(2025, 03, 18);
        DateTime date2 = new DateTime(2025, 03, 19);

        _service.AddProgression(id, date1, 60);

        Assert.Throws<ValidationException>(() => _service.AddProgression(id, date2, 50));
    }

    [Fact]
    public void UpdateTodoItem_WhenProgressAbove50_ShouldThrow()
    {
        int id = _service.CreateTodoItem("Test", "Initial Description", "Work");
        DateTime date1 = new DateTime(2025, 03, 18);
        DateTime date2 = new DateTime(2025, 03, 19);

        _service.AddProgression(id, date1, 30);
        _service.AddProgression(id, date2, 30);

        Assert.Throws<ValidationException>(() => _service.UpdateTodoItem(id, "New Description"));
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}