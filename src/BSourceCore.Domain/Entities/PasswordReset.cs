using BSourceCore.Domain.Enums;
using BSourceCore.Domain.Interfaces;

namespace BSourceCore.Domain.Entities;

public class PasswordReset : AuditEntity, ITenantEntity
{
    public Guid PasswordResetId { get; private set; }
    public Guid UserId { get; private set; }
    public string Token { get; private set; } = string.Empty;
    public DateTimeOffset ExpiresAt { get; private set; }
    public BaseStatus Status { get; private set; } = BaseStatus.Active;
    public Guid TenantId { get; set; }

    // Navegação
    public User User { get; private set; } = null!;
    public Tenant Tenant { get; private set; } = null!;

    private PasswordReset() { }

    public PasswordReset(Guid userId)
    {
        PasswordResetId = Guid.NewGuid();
        UserId = userId;
        Token = Guid.NewGuid().ToString("N");
        ExpiresAt = DateTimeOffset.UtcNow.AddMinutes(30);
        Status = BaseStatus.Active;
        SetCreatedAudit(null);
    }

    public bool IsValid()
    {
        return Status == BaseStatus.Active && ExpiresAt > DateTimeOffset.UtcNow;
    }

    public void Invalidate()
    {
        Status = BaseStatus.Inactive;
        SetUpdatedAudit(null);
    }
}
