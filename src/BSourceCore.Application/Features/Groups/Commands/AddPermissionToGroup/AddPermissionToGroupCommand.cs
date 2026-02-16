using MediatR;

namespace BSourceCore.Application.Features.Groups.Commands.AddPermissionToGroup;

public record AddPermissionToGroupCommand(Guid GroupId, Guid PermissionId) : IRequest<Unit>;
