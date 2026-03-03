using BSourceCore.Application.Features.Permissions.DTOs;
using BSourceCore.Shared.Kernel.Results;
using MediatR;

namespace BSourceCore.Application.Features.Permissions.Queries.GetPermissions;

public record GetPermissionsQuery : IRequest<Result<CollectionResult<PermissionDto>>>;
