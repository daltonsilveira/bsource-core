using BSourceCore.Application.Abstractions.Repositories;
using BSourceCore.Application.Features.Notifications.Commands.DeleteRecipient;
using BSourceCore.Shared.Abstractions;
using BSourceCore.Shared.Kernel.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BSourceCore.Application.Features.Notifications.Commands.UpdateRecipientWasRead;

public class DeleteRecipientCommandHandler : IRequestHandler<DeleteRecipientCommand, Result>
{
    private readonly INotificationRecipientRepository _notificationRecipientRepository;
    private readonly INotificationRepository _notificationRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;
    private readonly ILogger<DeleteRecipientCommandHandler> _logger;

    public DeleteRecipientCommandHandler(
        INotificationRecipientRepository notificationRecipientRepository,
        INotificationRepository notificationRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext,
        ILogger<DeleteRecipientCommandHandler> logger)
    {
        _notificationRecipientRepository = notificationRecipientRepository;
        _notificationRepository = notificationRepository;
        _unitOfWork = unitOfWork;
        _userContext = userContext;
        _logger = logger;
    }

    public async Task<Result> Handle(
        DeleteRecipientCommand request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting notification recipient: {UserId}", _userContext.UserId);

        var recipient = await _notificationRecipientRepository.GetByIdAsync(request.NotificationRecipientId, cancellationToken);

        if (recipient == null)
        {
            _logger.LogWarning("Notification recipient not found with Id: {NotificationRecipientId}", request.NotificationRecipientId);
            return Result.Fail(new Error(
                "NotificationRecipient.NotFound",
                $"Notification recipient with Id '{request.NotificationRecipientId}' not found",
                ErrorType.NotFound));
        };

        var notificationId = recipient.NotificationId;

        _notificationRecipientRepository.Delete(recipient);

        var otherRecipients = await _notificationRecipientRepository.ListByNotificationAsync(notificationId, cancellationToken);

        if (otherRecipients.Count().Equals(0))
        {
            var notification = await _notificationRepository.GetByIdAsync(notificationId, cancellationToken);

            if (notification is not null)
            {
                _notificationRepository.Delete(notification);
            }
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Notification recipient deleted: {UserId}", _userContext.UserId);

        return Result.Success();
    }
}
