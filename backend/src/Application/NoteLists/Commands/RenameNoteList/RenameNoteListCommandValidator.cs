namespace Backend.Application.NoteLists.Commands.RenameNoteList;

public class RenameNoteListCommandValidator : AbstractValidator<RenameNoteListCommand>
{
    public RenameNoteListCommandValidator()
    {
        RuleFor(v => v.SaveNoteList)
            .SetValidator(new SaveNoteListDtoValidator());
    }
}
