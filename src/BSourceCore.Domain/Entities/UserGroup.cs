using BSourceCore.Domain.Interfaces;

namespace BSourceCore.Domain.Entities;

public class UserGroup : AuditEntity, ITenantEntity
{
    public Guid UserGroupId { get; private set; }
    public Guid UserId { get; private set; }
    public Guid GroupId { get; private set; }
    public Guid TenantId { get; set; }

    // Navegação
    public User User { get; private set; } = null!;
    public Group Group { get; private set; } = null!;
    public Tenant Tenant { get; private set; } = null!;

    private UserGroup() { }

    public UserGroup(Guid userId, Guid groupId)
    {
        UserGroupId = Guid.NewGuid();
        UserId = userId;
        GroupId = groupId;
        SetCreatedAudit(null);
    }
}
