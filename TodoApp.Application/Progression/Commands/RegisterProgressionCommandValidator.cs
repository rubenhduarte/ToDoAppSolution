using FluentValidation;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.Progressions.Commands;
public class RegisterProgressionCommandValidator : AbstractValidator<RegisterProgressionCommand>
{
    public RegisterProgressionCommandValidator(TodoItem todoItem)
    {
        RuleFor(x => x.Percent)
            .GreaterThan(0).WithMessage("El porcentaje debe ser mayor a 0.")
            .LessThan(100).WithMessage("El porcentaje debe ser menor a 100.");

        RuleFor(x => x.DateTime)
            .Must(date =>
            {
                return !todoItem.Progressions.Any() || date > 
                            todoItem.Progressions.Max(p => p.Date);
            })
            .WithMessage("La fecha de la nueva progresión debe ser mayor que la última progresión.");

        RuleFor(x => x)
            .Must(cmd => todoItem.TotalProgress() + cmd.Percent <= 100)
            .WithMessage("La suma total de progresiones no puede exceder el 100%.");
    }
}