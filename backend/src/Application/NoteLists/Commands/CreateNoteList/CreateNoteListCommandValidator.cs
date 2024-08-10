namespace Backend.Application.NoteLists.Commands.CreateNoteList;

public class CreateNoteListCommandValidator : AbstractValidator<CreateNoteListCommand>
{
    public CreateNoteListCommandValidator()
    {
        RuleFor(v => v.Title)
            .NotEmpty()
            .MaximumLength(200);
    }
}
