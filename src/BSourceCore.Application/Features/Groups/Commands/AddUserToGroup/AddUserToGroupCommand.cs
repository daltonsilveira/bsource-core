using MediatR;

namespace BSourceCore.Application.Features.Groups.Commands.AddUserToGroup;

public record AddUserToGroupCommand(
    Guid GroupId,
    Guid UserId
) : IRequest<bool>;
