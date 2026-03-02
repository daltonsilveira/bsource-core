using MediatR;

namespace BSourceCore.Application.Features.Notifications.Commands.CreateNotification;

public record CreateNotificationCommand(
    string Title,
    string Message,
    string Data,
    List<Guid> UserIds,
    List<Guid> GroupIds,
    IEnumerable<CreateNotificationType> Types
) : IRequest<CreateNotificationResult>;

public record CreateNotificationResult();
