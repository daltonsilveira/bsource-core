using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BSourceCore.Domain.Enums;

namespace BSourceCore.Domain.Entities;

public class PasswordReset : TenantAuditEntity
{
    public Guid PasswordResetId { get; private set; } = Guid.NewGuid();
    [ForeignKey("User")]
    public Guid UserId { get; private set; }
    [Required, MaxLength(100)]
    public string Token { get; private set; } = string.Empty;
    [Required]
    public DateTimeOffset ExpiresAt { get; private set; }
    [Required]
    public BaseStatus Status { get; private set; } = BaseStatus.Active;

    // Navegação
    public User User { get; private set; } = null!;

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
