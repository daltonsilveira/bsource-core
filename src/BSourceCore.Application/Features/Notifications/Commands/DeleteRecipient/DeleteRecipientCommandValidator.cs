using FluentValidation;

namespace BSourceCore.Application.Features.Notifications.Commands.DeleteRecipient;

public class DeleteRecipientCommandValidator : AbstractValidator<DeleteRecipientCommand>
{
    public DeleteRecipientCommandValidator()
    {
        // NotificationId is optional: when null, marks all notifications as read for the user
    }
}
