using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BSourceCore.Domain.Interfaces;

namespace BSourceCore.Domain.Entities;

public class UserGroup : TenantAuditEntity
{
    public Guid UserGroupId { get; private set; } = Guid.NewGuid();
    [Required, ForeignKey("User")]
    public Guid UserId { get; private set; }
    [Required, ForeignKey("Group")]
    public Guid GroupId { get; private set; }

    // Navegação
    public User User { get; private set; } = null!;
    public Group Group { get; private set; } = null!;

    private UserGroup() { }

    public UserGroup(Guid userId, Guid groupId)
    {
        UserGroupId = Guid.NewGuid();
        UserId = userId;
        GroupId = groupId;
        SetCreatedAudit(null);
    }
}
