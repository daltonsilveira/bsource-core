using BSourceCore.Application.Features.Notifications.DTOs;
using BSourceCore.Shared.Kernel.Results;
using MediatR;

namespace BSourceCore.Application.Features.Notifications.Queries.GetNotifications;

public record GetNotificationsQuery() : IRequest<Result<CollectionResult<NotificationDto>>>;
