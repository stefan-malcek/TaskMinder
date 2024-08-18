namespace Backend.Application.NoteLists.Commands.MoveNoteList;

public record MoveNoteListDto
{
    public required Guid? ParentId { get; init; }
}
