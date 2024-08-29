using Backend.Domain.Common;

namespace Backend.Domain.Entities;

public class NoteList : BaseAuditableEntity
{
    public string Title { get; set; } = null!;
    public Guid? ParentId { get; set; }
    public NoteList? Parent { get; set; }
    public List<NoteList> Children { get; set; } = [];
    public List<Note> Notes { get; set; } = [];
}
