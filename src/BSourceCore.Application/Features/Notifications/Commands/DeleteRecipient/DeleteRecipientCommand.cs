using MediatR;

namespace BSourceCore.Application.Features.Notifications.Commands.DeleteRecipient;

public record DeleteRecipientCommand(
    Guid NotificationRecipientId
) : IRequest<DeleteRecipientResult>;
