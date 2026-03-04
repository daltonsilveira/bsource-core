using BSourceCore.Application.Features.Permissions.DTOs;
using BSourceCore.Shared.Kernel.Results;
using MediatR;

namespace BSourceCore.Application.Features.Permissions.Queries.ListPermissions;

public record ListPermissionsQuery : IRequest<Result<CollectionResult<PermissionDto>>>;
