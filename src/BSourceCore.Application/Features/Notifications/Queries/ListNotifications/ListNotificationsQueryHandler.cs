using BSourceCore.Application.Abstractions;
using BSourceCore.Application.Abstractions.Repositories;
using BSourceCore.Application.Features.Notifications.DTOs;
using BSourceCore.Application.Features.Users.DTOs;
using BSourceCore.Shared.Abstractions;
using BSourceCore.Shared.Kernel.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BSourceCore.Application.Features.Notifications.Queries.GetNotifications;

public class ListNotificationsQueryHandler : IRequestHandler<ListNotificationsQuery, Result<CollectionResult<NotificationDto>>>
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IUserContext _userContext;
    private readonly ILogger<ListNotificationsQueryHandler> _logger;

    public ListNotificationsQueryHandler(
        INotificationRepository notificationRepository,
        IUserContext userContext,
        ILogger<ListNotificationsQueryHandler> logger)
    {
        _notificationRepository = notificationRepository;
        _userContext = userContext;
        _logger = logger;
    }

    public async Task<Result<CollectionResult<NotificationDto>>> Handle(
        ListNotificationsQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting notifications for user: {UserId}", _userContext.UserId);

        var notifications = await _notificationRepository.ListByUserAsync(_userContext.UserId!.Value, cancellationToken);

        var items = notifications.Select(n => new NotificationDto(
            n.NotificationId,
            n.Title,
            n.Message,
            n.Data,
            n.Recipients.Any(r => r.UserId == _userContext.UserId && r.WasRead),
            n.CreatedAt,
            n.CreatedBy != null ? new UserAuditDto(n.CreatedBy.UserId, n.CreatedBy.Name) : null,
            n.Recipients.Where(r => r.UserId == _userContext.UserId).Select(r => r.NotificationRecipientId).FirstOrDefault())
            ).ToList();

        return Result<CollectionResult<NotificationDto>>.Success(CollectionResult<NotificationDto>.From(items));
    }
}
