namespace BSourceCore.API.Contracts.Responses;

public record NotificationResponse(
    Guid NotificationId,
    string Title,
    string Message,
    string Data,
    bool WasRead,
    DateTimeOffset CreatedAt);
