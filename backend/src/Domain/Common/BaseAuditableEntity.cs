namespace Backend.Domain.Common;

public abstract class BaseAuditableEntity : BaseCreatedAtEntity
{
    public Guid? CreatedBy { get; set; }

    public DateTimeOffset LastModifiedAt { get; set; }

    public Guid? LastModifiedBy { get; set; }
}
