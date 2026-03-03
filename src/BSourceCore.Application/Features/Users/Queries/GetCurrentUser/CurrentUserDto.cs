using BSourceCore.Application.Features.Notifications.DTOs;

namespace BSourceCore.Application.Features.Users.Queries.GetCurrentUser;

public record CurrentUserDto(
    Guid UserId,
    Guid TenantId,
    string Name,
    string Email,
    IEnumerable<string> PermissionCodes,
    IEnumerable<NotificationDto> Notifications);
