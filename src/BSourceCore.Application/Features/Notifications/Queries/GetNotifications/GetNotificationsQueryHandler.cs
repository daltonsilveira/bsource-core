using BSourceCore.Application.Abstractions;
using BSourceCore.Application.Abstractions.Repositories;
using BSourceCore.Application.Features.Notifications.DTOs;
using BSourceCore.Shared.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BSourceCore.Application.Features.Notifications.Queries.GetNotifications;

public class GetNotificationsQueryHandler : IRequestHandler<GetNotificationsQuery, IEnumerable<NotificationDto>>
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IUserContext _userContext;
    private readonly ILogger<GetNotificationsQueryHandler> _logger;

    public GetNotificationsQueryHandler(
        INotificationRepository notificationRepository,
        IUserContext userContext,
        ILogger<GetNotificationsQueryHandler> logger)
    {
        _notificationRepository = notificationRepository;
        _userContext = userContext;
        _logger = logger;
    }

    public async Task<IEnumerable<NotificationDto>> Handle(
        GetNotificationsQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting notifications for user: {UserId}", _userContext.UserId);

        var notifications = await _notificationRepository.ListByUserAsync(_userContext.UserId!.Value, cancellationToken);

        return notifications.Select(n => new NotificationDto(
            n.NotificationId,
            n.Title,
            n.Message,
            n.Data,
            n.Recipients.Any(r => r.UserId == _userContext.UserId && r.WasRead),
            n.CreatedAt));
    }
}
