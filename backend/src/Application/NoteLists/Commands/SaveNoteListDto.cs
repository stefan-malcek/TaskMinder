namespace Backend.Application.NoteLists.Commands;

public record SaveNoteListDto
{
    public required string Title { get; init; }
}
