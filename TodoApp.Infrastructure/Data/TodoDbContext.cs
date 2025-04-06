using Microsoft.EntityFrameworkCore;
using TodoApp.Domain.Entities;

namespace TodoApp.Infrastructure.Data;
public class TodoDbContext : DbContext
{
    public TodoDbContext(DbContextOptions<TodoDbContext> options)
        : base(options)
    {
    }

    public DbSet<TodoItem> TodoItems { get; set; }
    public DbSet<Progression> Progressions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuración de TodoItem:
        modelBuilder.Entity<TodoItem>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Title).IsRequired();
            entity.Property(t => t.Description).IsRequired();
            entity.Property(t => t.Category).IsRequired();

            // Mapeo de la colección privada _progressions
            entity.HasMany(t => t.Progressions)
                 .WithOne()
                .HasForeignKey("TodoItemId")
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configuración de Progression:
        modelBuilder.Entity<Progression>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Date).IsRequired();
            entity.Property(p => p.Percent).IsRequired();
        });
    }
}