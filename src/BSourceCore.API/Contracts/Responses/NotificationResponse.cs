using BSourceCore.Application.Features.Notifications.DTOs;

namespace BSourceCore.API.Contracts.Responses;

public class NotificationResponse
{
    public Guid NotificationId { get; }
    public string Title { get; }
    public string Message { get; }
    public string Data { get; }
    public bool WasRead { get; }
    public DateTimeOffset CreatedAt { get; }

    public NotificationResponse(NotificationDto dto)
    {
        NotificationId = dto.NotificationId;
        Title = dto.Title;
        Message = dto.Message;
        Data = dto.Data;
        WasRead = dto.WasRead;
        CreatedAt = dto.CreatedAt;
    }
}
