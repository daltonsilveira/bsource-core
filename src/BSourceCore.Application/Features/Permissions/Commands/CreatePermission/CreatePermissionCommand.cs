using BSourceCore.Application.Features.Permissions.DTOs;
using MediatR;

namespace BSourceCore.Application.Features.Permissions.Commands.CreatePermission;

public record CreatePermissionCommand(
    string Code,
    string Name,
    string? Description) : IRequest<PermissionDto>;
