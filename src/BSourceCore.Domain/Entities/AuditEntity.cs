using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BSourceCore.Domain.Interfaces;

namespace BSourceCore.Domain.Entities;

public abstract class AuditEntity
{
    [ForeignKey("CreatedBy")]
    public Guid? CreatedById { get; protected set; }
    [Required]
    public DateTimeOffset CreatedAt { get; protected set; }
    [ForeignKey("UpdatedBy")]
    public Guid? UpdatedById { get; protected set; }
    public DateTimeOffset? UpdatedAt { get; protected set; }

    // Navegação
    public virtual User? CreatedBy { get; protected set; } = null!;
    public virtual User? UpdatedBy { get; protected set; } = null!;

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
