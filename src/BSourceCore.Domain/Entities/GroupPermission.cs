using BSourceCore.Domain.Interfaces;

namespace BSourceCore.Domain.Entities;

public class GroupPermission : AuditEntity, ITenantEntity
{
    public Guid GroupPermissionId { get; private set; }
    public Guid GroupId { get; private set; }
    public Guid PermissionId { get; private set; }
    public Guid TenantId { get; set; }

    // Navegação
    public Group Group { get; private set; } = null!;
    public Permission Permission { get; private set; } = null!;
    public Tenant Tenant { get; private set; } = null!;

    private GroupPermission() { }

    public GroupPermission(Guid groupId, Guid permissionId)
    {
        GroupPermissionId = Guid.NewGuid();
        GroupId = groupId;
        PermissionId = permissionId;
        SetCreatedAudit(null);
    }
}
