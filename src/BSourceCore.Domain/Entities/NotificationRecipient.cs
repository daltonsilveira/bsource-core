using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BSourceCore.Domain.Entities;

public partial class NotificationRecipient : TenantAuditEntity
{
    public Guid NotificationRecipientId { get; set; } = Guid.NewGuid();
    [Required, ForeignKey("Notification")]
    public Guid NotificationId { get; set; }
    [Required, ForeignKey("User")]
    public Guid UserId { get; set; }
    public bool WasRead { get; set; } = false;

    // Navegação
    public virtual Notification Notification { get; set; } = null!;
    public virtual User User { get; set; } = null!;

    public NotificationRecipient() { }

    public NotificationRecipient(
        Guid notificationId,
        Guid userId,
        Guid? createdById)
    {
        NotificationId = notificationId;
        UserId = userId;
        SetCreatedAudit(createdById);
    }

    public void MarkAsRead(Guid? userId)
    {
        WasRead = true;
        SetUpdatedAudit(userId);
    }
}
