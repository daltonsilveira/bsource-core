using MediatR;

namespace BSourceCore.Application.Features.Notifications.Commands.CreateNotification;

public enum CreateNotificationType
{
    WebSocket = 1,
    Email = 2
}
