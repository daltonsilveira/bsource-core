using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BSourceCore.Domain.Interfaces;

namespace BSourceCore.Domain.Entities;

public partial class Notification : TenantAuditEntity
{
    public Guid NotificationId { get; set; } = Guid.NewGuid();
    [Required, MaxLength(200)]
    public string Title { get; set; } = string.Empty;
    [Required, MaxLength(8000)]
    public string Message { get; set; } = string.Empty;
    [MaxLength(8000)]
    public string Data { get; set; } = string.Empty;

    // Navegação
    public virtual IList<NotificationRecipient> Recipients { get; set; } = [];

    public Notification() { }

    public Notification(string title, string message, Guid? userId)
    {
        Title = title;
        Message = message;
        SetCreatedAudit(userId);
    }

    public Notification(string title, string message, string data, Guid? userId)
    {
        Title = title;
        Message = message;
        Data = data;
        SetCreatedAudit(userId);
    }
}
