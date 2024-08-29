using Backend.Domain.Common;

namespace Backend.Domain.Entities;

public class Note : BaseAuditableEntity
{
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public DateTimeOffset? Pinned { get; set; }
    public Guid? ListId { get; set; }
    public NoteList? List { get; set; }
}
