using FluentValidation;

namespace BSourceCore.Application.Features.Notifications.Commands.UpdateRecipientWasRead;

public class UpdateRecipientWasReadCommandValidator : AbstractValidator<UpdateRecipientWasReadCommand>
{
    public UpdateRecipientWasReadCommandValidator()
    {
        // NotificationId is optional: when null, marks all notifications as read for the user
    }
}
