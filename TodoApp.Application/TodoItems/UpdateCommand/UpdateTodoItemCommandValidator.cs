using FluentValidation;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.TodoItems.UpdateCommand;

public class UpdateTodoItemCommandValidator : AbstractValidator<UpdateTodoItemCommand>
{
    public UpdateTodoItemCommandValidator(TodoItem todoItem)
    {
        RuleFor(x => todoItem.TotalProgress())
            .LessThanOrEqualTo(50)
            .WithMessage("No se puede actualizar un item con más del 50% completado.");
    }
}