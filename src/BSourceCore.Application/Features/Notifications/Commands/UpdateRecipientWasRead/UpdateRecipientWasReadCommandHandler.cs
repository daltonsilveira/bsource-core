using BSourceCore.Application.Abstractions;
using BSourceCore.Application.Abstractions.Repositories;
using BSourceCore.Shared.Abstractions;
using BSourceCore.Shared.Kernel.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BSourceCore.Application.Features.Notifications.Commands.UpdateRecipientWasRead;

public class UpdateRecipientWasReadCommandHandler : IRequestHandler<UpdateRecipientWasReadCommand, Result>
{
    private readonly INotificationRecipientRepository _notificationRecipientRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;
    private readonly ILogger<UpdateRecipientWasReadCommandHandler> _logger;

    public UpdateRecipientWasReadCommandHandler(
        INotificationRecipientRepository notificationRecipientRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext,
        ILogger<UpdateRecipientWasReadCommandHandler> logger)
    {
        _notificationRecipientRepository = notificationRecipientRepository;
        _unitOfWork = unitOfWork;
        _userContext = userContext;
        _logger = logger;
    }

    public async Task<Result> Handle(
        UpdateRecipientWasReadCommand request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating notification recipient was read: {UserId}", _userContext.UserId);

        if(request.NotificationId is not null)
        {
            var recipient = await _notificationRecipientRepository.GetByNotificationAndUserAsync(request.NotificationId.Value, _userContext.UserId.Value, cancellationToken);
            if (recipient is null)
            {
                return Result.Fail(new Error(
                    "NotificationRecipient.NotFound",
                    $"Notification recipient with Notification Id '{request.NotificationId}' and User Id '{_userContext.UserId}' not found",
                    ErrorType.NotFound));                    
            }

            recipient.MarkAsRead(_userContext.UserId);
            _notificationRecipientRepository.Update(recipient);
        }
        else
        {
            var recipients = await _notificationRecipientRepository.ListByUserAsync(_userContext.UserId.Value, cancellationToken);

            foreach (var recipient in recipients)
            {
                recipient.MarkAsRead(_userContext.UserId);
                _notificationRecipientRepository.Update(recipient);
            }
        }
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Notification recipient was read updated: {UserId}", _userContext.UserId);

        return Result.Success();
    }
}
