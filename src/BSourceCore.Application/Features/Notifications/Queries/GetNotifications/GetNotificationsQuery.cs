using BSourceCore.Application.Features.Notifications.DTOs;
using MediatR;

namespace BSourceCore.Application.Features.Notifications.Queries.GetNotifications;

public record GetNotificationsQuery() : IRequest<IEnumerable<NotificationDto>>;
