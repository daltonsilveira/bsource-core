using BSourceCore.Application.Features.Permissions.DTOs;
using MediatR;

namespace BSourceCore.Application.Features.Permissions.Commands.UpdatePermission;

public record UpdatePermissionCommand(
    Guid PermissionId,
    string Name,
    string? Description) : IRequest<PermissionDto>;
