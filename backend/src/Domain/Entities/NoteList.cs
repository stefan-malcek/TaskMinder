using Backend.Domain.Common;

namespace Backend.Domain.Entities;

public class NoteList : BaseAuditableEntity
{
    public string Title { get; set; } = null!;
}
