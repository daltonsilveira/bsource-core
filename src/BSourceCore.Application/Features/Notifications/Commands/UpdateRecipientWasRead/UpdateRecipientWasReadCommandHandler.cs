using BSourceCore.Application.Abstractions;
using BSourceCore.Application.Abstractions.Repositories;
using BSourceCore.Shared.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BSourceCore.Application.Features.Notifications.Commands.UpdateRecipientWasRead;

public class UpdateRecipientWasReadCommandHandler : IRequestHandler<UpdateRecipientWasReadCommand, UpdateRecipientWasReadResult>
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

    public async Task<UpdateRecipientWasReadResult> Handle(
        UpdateRecipientWasReadCommand request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating notification recipient was read: {UserId}", _userContext.UserId);

        if(request.NotificationId is not null)
        {
            var recipient = await _notificationRecipientRepository.GetByNotificationAsync(request.NotificationId.Value, _userContext.UserId.Value, cancellationToken);
            if (recipient is null)
            {
                throw new KeyNotFoundException($"Notification recipient with Notification Id '{request.NotificationId}' and User Id '{_userContext.UserId}' not found");
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

        return new UpdateRecipientWasReadResult();
    }
}
