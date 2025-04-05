using TodoApp.Application.Services;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Interfaces;
using TodoApp.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorUI", policy =>
    {
        policy.WithOrigins("https://localhost:7041")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Registro de dependencias utilizando la implementación en memoria
builder.Services.AddSingleton<ITodoListRepository, InMemoryTodoListRepository>();
builder.Services.AddSingleton<ITodoList, TodoList>(sp =>
    new TodoList(sp.GetRequiredService<ITodoListRepository>()));
builder.Services.AddSingleton<TodoListService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors("AllowBlazorUI");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();