namespace BSourceCore.Application.Features.Notifications.DTOs;

public record NotificationDto(
    Guid NotificationId,    
    string Title,
    string Message,
    string Data,
    bool WasRead,
    DateTimeOffset CreatedAt,
    Guid? NotificationRecipientId);
