using Backend.Domain.Common;
using Backend.Domain.Enums;

namespace Backend.Domain.Entities;

public class AuditTrail : BaseEntity
{
    public Guid? UserId { get; set; }

    public TrailType TrailType { get; set; }

    public DateTimeOffset AuditedAt { get; set; }

    public required string EntityName { get; set; }

    public string? PrimaryKey { get; set; }

    public Dictionary<string, object?> OldValues { get; set; } = new();

    public Dictionary<string, object?> NewValues { get; set; } = new();

    public List<string> ChangedColumns { get; set; } = new();
}
