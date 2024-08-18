namespace Backend.Application.NoteLists.Commands.RenameNoteList;

public class RenameNoteListCommandValidator : AbstractValidator<RenameNoteListCommand>
{
    public RenameNoteListCommandValidator()
    {
        RuleFor(v => v.CreateNoteList)
            .SetValidator(new SaveNoteListDtoValidator());
    }
}
