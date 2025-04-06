using FluentValidation;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.TodoItems.UpdateCommand;

public class UpdateTodoItemCommandValidator : AbstractValidator<UpdateTodoItemCommand>
{
    public UpdateTodoItemCommandValidator(TodoItem todoItem)
    {
        RuleFor(x => x.NewDescription).NotEmpty();
        RuleFor(x => x.TodoItemId).Must(_ => todoItem.TotalProgress() <= 50)
                                  .WithMessage("Más del 50% completado, no se puede modificar");
    }
}