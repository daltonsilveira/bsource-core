using BSourceCore.Shared.Kernel.Results;
using MediatR;

namespace BSourceCore.Application.Features.Groups.Commands.RemovePermissionFromGroup;

public record RemovePermissionFromGroupCommand(Guid GroupId, Guid PermissionId) : IRequest<Result>;
