using BSourceCore.Application.Features.Permissions.DTOs;
using MediatR;

namespace BSourceCore.Application.Features.Permissions.Queries.GetPermissionById;

public record GetPermissionByIdQuery(Guid PermissionId) : IRequest<PermissionDto?>;
