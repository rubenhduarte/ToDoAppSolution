using Microsoft.EntityFrameworkCore;
using TodoApp.Infrastructure.Data;

namespace TodoApp.Tests.ContextFactory;

public static class TestDbContextFactory
{
   public static TodoDbContext CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<TodoDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var context = new TodoDbContext(options);
        context.Database.EnsureCreated();
        return context;
    }
}