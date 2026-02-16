using BSourceCore.Domain.Enums;
using BSourceCore.Domain.Interfaces;

namespace BSourceCore.Domain.Entities;

public class Group : AuditEntity, ITenantEntity
{
    public Guid GroupId { get; private set; }
    public Guid TenantId { get; set; }
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public BaseStatus Status { get; private set; } = BaseStatus.Active;

    // Navegação
    public Tenant Tenant { get; private set; } = null!;
    public ICollection<UserGroup> UserGroups { get; private set; } = new List<UserGroup>();
    public ICollection<GroupPermission> GroupPermissions { get; private set; } = new List<GroupPermission>();

    private Group() { }

    public Group(Guid tenantId, string name, string? description = null)
    {
        GroupId = Guid.NewGuid();
        TenantId = tenantId;
        Name = name;
        Description = description;
        Status = BaseStatus.Active;
        SetCreatedAudit(null);
    }

    public void Update(string name, string? description, Guid? userId = null)
    {
        Name = name;
        Description = description;
        SetUpdatedAudit(userId);
    }

    public void SetStatus(BaseStatus status, Guid? userId = null)
    {
        Status = status;
        SetUpdatedAudit(userId);
    }
}
