using BSourceCore.Application.Abstractions;
using BSourceCore.Application.Features.Permissions.DTOs;
using BSourceCore.Shared.Abstractions;
using MediatR;

namespace BSourceCore.Application.Features.Permissions.Queries.GetPermissionById;

public class GetPermissionByIdQueryHandler : IRequestHandler<GetPermissionByIdQuery, PermissionDto?>
{
    private readonly IPermissionRepository _permissionRepository;
    private readonly IUserContext _userContext;

    public GetPermissionByIdQueryHandler(
        IPermissionRepository permissionRepository,
        IUserContext userContext)
    {
        _permissionRepository = permissionRepository;
        _userContext = userContext;
    }

    public async Task<PermissionDto?> Handle(GetPermissionByIdQuery request, CancellationToken cancellationToken)
    {
        var permission = await _permissionRepository.GetByIdAsync(request.PermissionId, cancellationToken);

        if (permission is null)
        {
            return null;
        }

        return new PermissionDto(
            permission.PermissionId,
            permission.Code,
            permission.Name,
            permission.Description,
            permission.Status.ToString(),
            permission.CreatedAt);
    }
}
