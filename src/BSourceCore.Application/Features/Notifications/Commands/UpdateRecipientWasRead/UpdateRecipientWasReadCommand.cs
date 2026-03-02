using MediatR;

namespace BSourceCore.Application.Features.Notifications.Commands.UpdateRecipientWasRead;

public record UpdateRecipientWasReadCommand(
    Guid? NotificationId
) : IRequest<UpdateRecipientWasReadResult>;
