namespace TodoApp.Application.TodoItems.CreateCommand;
using FluentValidation;
public class CreateTodoItemCommandValidator : AbstractValidator<CreateTodoItemCommand>
{
    public CreateTodoItemCommandValidator(ITodoListRepository repository)
    {
        RuleFor(x => x.Category)
            .Must(category => repository.GetAllCategories().Contains(category))
            .WithMessage("La categoría no es válida.");
    }
}