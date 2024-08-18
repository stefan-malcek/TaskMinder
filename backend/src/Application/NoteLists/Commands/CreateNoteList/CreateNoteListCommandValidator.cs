namespace Backend.Application.NoteLists.Commands.CreateNoteList;

public class CreateNoteListCommandValidator : AbstractValidator<CreateNoteListCommand>
{
    public CreateNoteListCommandValidator()
    {
        RuleFor(v => v.SaveNoteList)
            .SetValidator(new SaveNoteListDtoValidator());
    }
}
