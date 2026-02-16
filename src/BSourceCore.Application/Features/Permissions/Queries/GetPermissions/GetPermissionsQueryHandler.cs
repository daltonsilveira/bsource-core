using BSourceCore.Application.Abstractions;
using BSourceCore.Application.Features.Permissions.DTOs;
using BSourceCore.Shared.Abstractions;
using MediatR;

namespace BSourceCore.Application.Features.Permissions.Queries.GetPermissions;

public class GetPermissionsQueryHandler : IRequestHandler<GetPermissionsQuery, IEnumerable<PermissionDto>>
{
    private readonly IPermissionRepository _permissionRepository;
    private readonly IUserContext _userContext;

    public GetPermissionsQueryHandler(
        IPermissionRepository permissionRepository,
        IUserContext userContext)
    {
        _permissionRepository = permissionRepository;
        _userContext = userContext;
    }

    public async Task<IEnumerable<PermissionDto>> Handle(GetPermissionsQuery request, CancellationToken cancellationToken)
    {
        var permissions = await _permissionRepository.GetAllAsync(cancellationToken);

        return permissions.Select(p => new PermissionDto(
            p.PermissionId,
            p.Code,
            p.Name,
            p.Description,
            p.Status.ToString(),
            p.CreatedAt));
    }
}
