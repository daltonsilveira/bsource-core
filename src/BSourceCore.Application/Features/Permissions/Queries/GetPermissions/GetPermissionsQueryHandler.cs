using BSourceCore.Application.Abstractions;
using BSourceCore.Application.Abstractions.Repositories;
using BSourceCore.Application.Features.Permissions.DTOs;
using BSourceCore.Shared.Abstractions;
using BSourceCore.Shared.Kernel.Results;
using MediatR;

namespace BSourceCore.Application.Features.Permissions.Queries.GetPermissions;

public class GetPermissionsQueryHandler : IRequestHandler<GetPermissionsQuery, Result<CollectionResult<PermissionDto>>>
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

    public async Task<Result<CollectionResult<PermissionDto>>> Handle(GetPermissionsQuery request, CancellationToken cancellationToken)
    {
        var permissions = await _permissionRepository.GetAllAsync(cancellationToken);

        var items = permissions.Select(p => new PermissionDto(
            p.PermissionId,
            p.Code,
            p.Name,
            p.Description,
            p.Status.ToString(),
            p.CreatedAt)).ToList();

        return Result<CollectionResult<PermissionDto>>.Success(new CollectionResult<PermissionDto>
        {
            Results = items,
            Total = items.Count
        });
    }
}
