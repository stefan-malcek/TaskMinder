namespace Backend.Application.NoteLists.Commands;

public class SaveNoteListDtoValidator : AbstractValidator<SaveNoteListDto>
{
    public SaveNoteListDtoValidator()
    {
        RuleFor(v => v.Title)
            .NotEmpty()
            .MaximumLength(200);
    }
}
