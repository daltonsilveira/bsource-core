using System.ComponentModel.DataAnnotations;
using BSourceCore.Domain.Enums;

namespace BSourceCore.Domain.Entities;

public class Tenant : AuditEntity
{
    public Guid TenantId { get; private set; }
    [Required, MaxLength(200)]
    public string Name { get; private set; } = string.Empty;
    [Required, MaxLength(100)]
    public string Slug { get; private set; } = string.Empty;
    [MaxLength(500)]
    public string Description { get; private set; } = string.Empty;
    [Required]
    public BaseStatus Status { get; private set; } = BaseStatus.Active;

    // Navegação
    public ICollection<User> Users { get; private set; } = new List<User>();
    public ICollection<Group> Groups { get; private set; } = new List<Group>();
    public ICollection<Permission> Permissions { get; private set; } = new List<Permission>();

    private Tenant() { }

    public Tenant(string name, string slug, string description = "")
    {
        TenantId = Guid.NewGuid();
        Name = name;
        Slug = slug;
        Description = description;
        Status = BaseStatus.Active;
        SetCreatedAudit(null);
    }

    public void Update(string name, string slug, string description = "")
    {
        Name = name;
        Slug = slug;
        Description = description;
        SetUpdatedAudit(null);
    }

    public void SetStatus(BaseStatus status, Guid? userId = null)
    {
        Status = status;
        SetUpdatedAudit(userId);
    }
}
