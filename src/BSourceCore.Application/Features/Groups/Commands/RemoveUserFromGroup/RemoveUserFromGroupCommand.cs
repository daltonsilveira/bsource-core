using BSourceCore.Shared.Kernel.Results;
using MediatR;

namespace BSourceCore.Application.Features.Groups.Commands.RemoveUserFromGroup;

public record RemoveUserFromGroupCommand(
    Guid GroupId,
    Guid UserId
) : IRequest<Result>;
