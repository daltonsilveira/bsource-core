using BSourceCore.Application.Abstractions;
using BSourceCore.Application.Abstractions.Repositories;
using BSourceCore.Application.Features.Permissions.DTOs;
using BSourceCore.Shared.Abstractions;
using BSourceCore.Shared.Kernel.Results;
using MediatR;

namespace BSourceCore.Application.Features.Permissions.Queries.GetPermissionById;

public class GetPermissionByIdQueryHandler : IRequestHandler<GetPermissionByIdQuery, Result<PermissionDto>>
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

    public async Task<Result<PermissionDto>> Handle(GetPermissionByIdQuery request, CancellationToken cancellationToken)
    {
        var permission = await _permissionRepository.GetByIdAsync(request.PermissionId, cancellationToken);

        if (permission is null)
        {
            return Result<PermissionDto>.Fail(new Error(
                "Permission.NotFound",
                $"Permission with Id '{request.PermissionId}' not found",
                ErrorType.NotFound));
        }

        return Result<PermissionDto>.Success(new PermissionDto(
            permission.PermissionId,
            permission.Code,
            permission.Name,
            permission.Description,
            permission.Status.ToString(),
            permission.CreatedAt));
    }
}
