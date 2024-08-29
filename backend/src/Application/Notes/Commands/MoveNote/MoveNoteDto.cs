namespace Backend.Application.Notes.Commands.MoveNote;

public record MoveNoteDto
{
    public required Guid ListId { get; init; }
}
