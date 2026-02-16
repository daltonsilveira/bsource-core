namespace BSourceCore.Domain.Entities;

public abstract class AuditEntity
{
    public Guid? CreatedById { get; protected set; }
    public DateTimeOffset CreatedAt { get; protected set; }
    public Guid? UpdatedById { get; protected set; }
    public DateTimeOffset? UpdatedAt { get; protected set; }

    protected void SetCreatedAudit(Guid? userId)
    {
        CreatedById = userId;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    protected void SetUpdatedAudit(Guid? userId)
    {
        UpdatedById = userId;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}
