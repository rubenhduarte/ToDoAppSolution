using FluentValidation;
using TodoApp.Application.Services;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Interfaces;
using TodoApp.Infrastructure.Repositories;

namespace TodoApp.Tests;
public class TodoListTests
{
    private readonly ITodoListRepository _repository;
    private readonly ITodoList _todoList;
    private readonly TodoListService _service;

    public TodoListTests()
    {
        // Configuración para pruebas en memoria
        _repository = new InMemoryTodoListRepository();
        _todoList = new TodoList(_repository);
        _service = new TodoListService(_todoList, _repository);
    }

    [Fact]
    public void CreateTodoItem_HappyPath()
    {
        // Arrange
        string title = "Complete Project Report";
        string description = "Finish the final report for the project";
        string category = "Work";

        // Act
        int id = _service.CreateTodoItem(title, description, category);

        // Assert: Verificar que se haya creado el TodoItem con las propiedades correctas.
        var item = _todoList.GetItemById(id);
        Assert.NotNull(item);
        Assert.Equal(title, item.Title);
        Assert.Equal(description, item.Description);
        Assert.Equal(category, item.Category);
    }

    [Fact]
    public void RegisterProgression_HappyPath()
    {
        // Arrange
        int id = _service.CreateTodoItem("Test", "Test Desc", "Work");
        DateTime date1 = new DateTime(2025, 03, 18);
        DateTime date2 = new DateTime(2025, 03, 19);

        // Act
        _service.AddProgression(id, date1, 30);
        _service.AddProgression(id, date2, 50);

        // Assert: Verificar que se hayan agregado las progresiones y el total sea el esperado.
        var item = _todoList.GetItemById(id);
        Assert.Equal(2, item.Progressions.Count);
        Assert.Equal(30, item.Progressions[0].Percent);
        Assert.Equal(50, item.Progressions[1].Percent);
        Assert.Equal(80, item.TotalProgress());
    }

    [Fact]
    public void RegisterProgression_InvalidDate_ShouldThrow()
    {
        // Arrange
        int id = _service.CreateTodoItem("Test", "Test Desc", "Work");
        DateTime date1 = new DateTime(2025, 03, 18);
        DateTime invalidDate = new DateTime(2025, 03, 17); // Fecha anterior a la primera progresión

        _service.AddProgression(id, date1, 30);

        // Act & Assert: Se espera que agregar una progresión con fecha inválida lance excepción.
        Assert.Throws<ValidationException>(() => _service.AddProgression(id, invalidDate, 20));
    }

    [Fact]
    public void RegisterProgression_ExceedTotalProgress_ShouldThrow()
    {
        // Arrange
        int id = _service.CreateTodoItem("Test", "Test Desc", "Work");
        DateTime date1 = new DateTime(2025, 03, 18);
        DateTime date2 = new DateTime(2025, 03, 19);

        _service.AddProgression(id, date1, 60);

        // Act & Assert: Al intentar agregar una progresión que exceda el 100% total se debe lanzar excepción.
        Assert.Throws<ValidationException>(() => _service.AddProgression(id, date2, 50));
    }

    [Fact]
    public void UpdateTodoItem_WhenProgressAbove50_ShouldThrow()
    {
        // Arrange
        int id = _service.CreateTodoItem("Test", "Initial Description", "Work");
        DateTime date1 = new DateTime(2025, 03, 18);
        DateTime date2 = new DateTime(2025, 03, 19);

        _service.AddProgression(id, date1, 30);
        _service.AddProgression(id, date2, 30); // Total = 60%

        // Act & Assert: Intentar actualizar un item con más del 50% de progreso debe lanzar excepción.
        Assert.Throws<ValidationException>(() => _service.UpdateTodoItem(id, "New Description"));
    }

    [Fact]
    public void RemoveTodoItem_WhenProgressAbove50_ShouldThrow()
    {
        // Arrange
        int id = _service.CreateTodoItem("Test", "Initial Description", "Work");
        DateTime date1 = new DateTime(2025, 03, 18);
        DateTime date2 = new DateTime(2025, 03, 19);

        _service.AddProgression(id, date1, 30);
        _service.AddProgression(id, date2, 30); // Total = 60%

        // Act & Assert: Intentar remover un item con más del 50% de progreso debe lanzar excepción.
        Assert.Throws<ValidationException>(() => _service.RemoveTodoItem(id));
    }
}