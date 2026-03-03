using BSourceCore.Shared.Kernel.Results;
using MediatR;

namespace BSourceCore.Application.Features.Notifications.Commands.DeleteRecipient;

public record DeleteRecipientCommand(
    Guid NotificationRecipientId
) : IRequest<Result>;
