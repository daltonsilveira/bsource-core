using BSourceCore.Application.Abstractions;
using BSourceCore.Application.Abstractions.Repositories;
using BSourceCore.Application.Features.Notifications.DTOs;
using BSourceCore.Application.Features.Users.DTOs;
using BSourceCore.Shared.Abstractions;
using BSourceCore.Shared.Kernel.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BSourceCore.Application.Features.Notifications.Queries.GetNotificationById;

public class GetNotificationByIdQueryHandler : IRequestHandler<GetNotificationByIdQuery, Result<NotificationDto>>
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IUserContext _userContext;
    private readonly ILogger<GetNotificationByIdQueryHandler> _logger;

    public GetNotificationByIdQueryHandler(
        INotificationRepository notificationRepository,
        IUserContext userContext,
        ILogger<GetNotificationByIdQueryHandler> logger)
    {
        _notificationRepository = notificationRepository;
        _userContext = userContext;
        _logger = logger;
    }

    public async Task<Result<NotificationDto>> Handle(
        GetNotificationByIdQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting notification by Id: {NotificationId}", request.NotificationId);

        var notification = await _notificationRepository.GetByIdWithRecipientsAsync(request.NotificationId, cancellationToken);

        if (notification is null)
        {
            _logger.LogWarning("Notification not found with Id: {NotificationId}", request.NotificationId);
            return Result<NotificationDto>.Fail(new Error(
                "Notification.NotFound",
                $"Notification with Id '{request.NotificationId}' not found",
                ErrorType.NotFound));
        }

        return Result<NotificationDto>.Success(new NotificationDto(
            notification.NotificationId,
            notification.Title,
            notification.Message,
            notification.Data,
            notification.Recipients.Any(r => r.UserId == _userContext.UserId && r.WasRead),
            notification.CreatedAt,
            notification.CreatedBy != null ? new UserAuditDto(notification.CreatedBy.UserId, notification.CreatedBy.Name) : null,
            notification.Recipients.Where(r => r.UserId == _userContext.UserId).Select(r => r.NotificationRecipientId).FirstOrDefault())
            );
    }
}
