using System.ComponentModel.DataAnnotations;
using BSourceCore.Domain.Enums;
using BSourceCore.Domain.Interfaces;

namespace BSourceCore.Domain.Entities;

public class Permission : TenantAuditEntity
{
    public Guid PermissionId { get; private set; } = Guid.NewGuid();    
    [Required, MaxLength(100)]
    public string Code { get; private set; } = string.Empty;
    [Required, MaxLength(200)]
    public string Name { get; private set; } = string.Empty;
    [MaxLength(500)]
    public string Description { get; private set; } = string.Empty;
    [Required]
    public BaseStatus Status { get; private set; } = BaseStatus.Active;

    // Navegação
    public ICollection<GroupPermission> GroupPermissions { get; private set; } = new List<GroupPermission>();

    private Permission() { }

    public Permission(Guid tenantId, string code, string name, string description = "")
    {
        PermissionId = Guid.NewGuid();
        TenantId = tenantId;
        Code = code;
        Name = name;
        Description = description;
        Status = BaseStatus.Active;
        SetCreatedAudit(null);
    }

    public void Update(string code, string name, string description = "", Guid? userId = null)
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
