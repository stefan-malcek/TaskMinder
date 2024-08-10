namespace Backend.Domain.Common;

public abstract class BaseCreatedAtEntity : BaseEntity
{
    public DateTimeOffset CreatedAt { get; set; }
}
