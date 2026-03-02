using MediatR;

namespace BSourceCore.Application.Features.Groups.Commands.CreateGroup;

public record CreateGroupCommand(
    Guid TenantId,
    string Name,
    string? Description
) : IRequest<CreateGroupResult>;
