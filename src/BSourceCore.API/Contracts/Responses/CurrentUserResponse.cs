namespace BSourceCore.API.Contracts.Responses;

public record CurrentUserResponse(
    Guid UserId,
    string Email,
    string Name,
    Guid TenantId,
    IEnumerable<string> PermissionCodes,
    IEnumerable<NotificationResponse> Notifications);
