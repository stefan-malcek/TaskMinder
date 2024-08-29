namespace Backend.Application.Notes.Commands.UpdateNote;

public class UpdateNoteCommandValidator : AbstractValidator<UpdateNoteCommand>
{
    public UpdateNoteCommandValidator()
    {
        RuleFor(v => v.SaveNote)
            .SetValidator(new SaveNoteDtoValidator());
    }
}
