namespace Backend.Application.Notes.Commands.CreateNote;

public class CreateNoteCommandValidator : AbstractValidator<CreateNoteCommand>
{
    public CreateNoteCommandValidator()
    {
        RuleFor(v => v.SaveNote)
            .SetValidator(new SaveNoteDtoValidator());
    }
}
