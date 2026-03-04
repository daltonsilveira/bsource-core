using BSourceCore.Application.Features.Permissions.DTOs;
using BSourceCore.Shared.Kernel.Results;
using MediatR;

namespace BSourceCore.Application.Features.Permissions.Queries.GetPermissionById;

public record GetPermissionByIdQuery(Guid PermissionId) : IRequest<Result<PermissionDto>>;
