using MediatR;

namespace BSourceCore.Application.Features.Groups.Commands.UpdateGroup;

public record UpdateGroupCommand(
    Guid GroupId,
    string Name,
    string? Description
) : IRequest<UpdateGroupResult>;
