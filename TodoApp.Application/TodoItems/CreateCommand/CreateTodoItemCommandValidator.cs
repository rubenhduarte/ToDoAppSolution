namespace TodoApp.Application.TodoItems.CreateCommand;
using FluentValidation;
using TodoApp.Domain.Interfaces;

public class CreateTodoItemCommandValidator : AbstractValidator<CreateTodoItemCommand>
{
    public CreateTodoItemCommandValidator(ITodoListRepository repository)
    {
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
        RuleFor(x => x.Category).Must(cat => repository.GetAllCategories().Contains(cat))
                                .WithMessage("Categoría inválida");
    }
}