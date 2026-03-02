using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BSourceCore.Domain.Interfaces;

namespace BSourceCore.Domain.Entities;

public class GroupPermission : TenantAuditEntity
{
    public Guid GroupPermissionId { get; private set; } = Guid.NewGuid();
    [Required, ForeignKey("Group")]
    public Guid GroupId { get; private set; }
    [Required, ForeignKey("Permission")]
    public Guid PermissionId { get; private set; }

    // Navegação
    public virtual Group Group { get; private set; } = null!;
    public virtual Permission Permission { get; private set; } = null!;

    private GroupPermission() { }

    public GroupPermission(Guid groupId, Guid permissionId)
    {
        GroupPermissionId = Guid.NewGuid();
        GroupId = groupId;
        PermissionId = permissionId;
        SetCreatedAudit(null);
    }
}
