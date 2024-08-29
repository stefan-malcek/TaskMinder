namespace Backend.Application.Notes.Commands;

public record SaveNoteDto
{
    public required string Title { get; init; }
    public required string Content { get; init; }
}
