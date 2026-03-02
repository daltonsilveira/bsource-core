using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BSourceCore.Domain.Interfaces;

namespace BSourceCore.Domain.Entities;

public abstract class TenantAuditEntity : AuditEntity, ITenantEntity
{
    [Required, ForeignKey("Tenant")]
    public Guid TenantId { get; set; }

    // Navegação
    public virtual Tenant Tenant { get; protected set; } = null!;
}
