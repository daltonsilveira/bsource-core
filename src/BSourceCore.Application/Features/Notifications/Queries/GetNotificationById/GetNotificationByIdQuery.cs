using BSourceCore.Application.Features.Notifications.DTOs;
using MediatR;

namespace BSourceCore.Application.Features.Notifications.Queries.GetNotificationById;

public record GetNotificationByIdQuery(Guid NotificationId) : IRequest<NotificationDto?>;
