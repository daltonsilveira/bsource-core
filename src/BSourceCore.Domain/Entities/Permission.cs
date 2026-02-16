using BSourceCore.Domain.Enums;
using BSourceCore.Domain.Interfaces;

namespace BSourceCore.Domain.Entities;

public class Permission : AuditEntity, ITenantEntity
{
    public Guid PermissionId { get; private set; }    
    public string Code { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public BaseStatus Status { get; private set; } = BaseStatus.Active;
    public Guid TenantId { get; set; }

    // Navegação
    public Tenant Tenant { get; private set; } = null!;
    public ICollection<GroupPermission> GroupPermissions { get; private set; } = new List<GroupPermission>();

    private Permission() { }

    public Permission(Guid tenantId, string code, string name, string? description = null)
    {
        PermissionId = Guid.NewGuid();
        TenantId = tenantId;
        Code = code;
        Name = name;
        Description = description;
        Status = BaseStatus.Active;
        SetCreatedAudit(null);
    }

    public void Update(string code, string name, string? description, Guid? userId = null)
    {
        Code = code;
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
