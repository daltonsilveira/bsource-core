using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BSourceCore.Domain.Enums;
using BSourceCore.Domain.Interfaces;

namespace BSourceCore.Domain.Entities;

public class Group : TenantAuditEntity
{
    public Guid GroupId { get; private set; } = Guid.NewGuid();    
    [Required, MaxLength(200)]
    public string Name { get; private set; } = string.Empty;
    [MaxLength(500)]
    public string Description { get; private set; } = string.Empty;
    [Required]
    public BaseStatus Status { get; private set; } = BaseStatus.Active;

    // Navegação
    public virtual ICollection<UserGroup> UserGroups { get; private set; } = new List<UserGroup>();
    public virtual ICollection<GroupPermission> GroupPermissions { get; private set; } = new List<GroupPermission>();

    private Group() { }

    public Group(Guid tenantId, string name, string description)
    {
        GroupId = Guid.NewGuid();
        TenantId = tenantId;
        Name = name;
        Description = description;
        Status = BaseStatus.Active;
        SetCreatedAudit(null);
    }

    public void Update(string name, string description)
    {
        Name = name;
        Description = description;
        SetUpdatedAudit(null);
    }

    public void SetStatus(BaseStatus status)
    {
        Status = status;
        SetUpdatedAudit(null);
    }
}
