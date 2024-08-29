namespace Backend.Application.Notes.Commands;

public class SaveNoteDtoValidator : AbstractValidator<SaveNoteDto>
{
    public SaveNoteDtoValidator()
    {
        RuleFor(v => v.Title)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(v => v.Content)
            .NotNull();
    }
}
