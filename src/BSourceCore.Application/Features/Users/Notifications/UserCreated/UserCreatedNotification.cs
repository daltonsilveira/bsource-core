using MediatR;

namespace BSourceCore.Application.Features.Users.Notifications.UserCreated;

public record UserCreatedNotification(
    Guid UserId,
    Guid TenantId,
    string Name,
    string Email,
    string Password
) : INotification;
