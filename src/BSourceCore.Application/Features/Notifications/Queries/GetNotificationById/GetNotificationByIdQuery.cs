using BSourceCore.Application.Features.Notifications.DTOs;
using BSourceCore.Shared.Kernel.Results;
using MediatR;

namespace BSourceCore.Application.Features.Notifications.Queries.GetNotificationById;

public record GetNotificationByIdQuery(Guid NotificationId) : IRequest<Result<NotificationDto>>;
