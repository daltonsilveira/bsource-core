using BSourceCore.Application.Features.Permissions.DTOs;
using MediatR;

namespace BSourceCore.Application.Features.Permissions.Queries.GetPermissions;

public record GetPermissionsQuery : IRequest<IEnumerable<PermissionDto>>;
