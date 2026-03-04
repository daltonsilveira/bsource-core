using BSourceCore.Application.Features.Permissions.DTOs;
using BSourceCore.Shared.Kernel.Results;
using MediatR;

namespace BSourceCore.Application.Features.Permissions.Commands.CreatePermission;

public record CreatePermissionCommand(
    string Code,
    string Name,
    string? Description) : IRequest<Result<PermissionDto>>;
