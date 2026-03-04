using BSourceCore.Application.Abstractions;
using BSourceCore.Application.Abstractions.Repositories;
using BSourceCore.Application.Features.Permissions.DTOs;
using BSourceCore.Application.Features.Permissions.Queries.GetPermissionById;
using BSourceCore.Shared.Abstractions;
using BSourceCore.Shared.Kernel.Results;
using MediatR;

namespace BSourceCore.Application.Features.Permissions.Commands.UpdatePermission;

public class UpdatePermissionCommandHandler : IRequestHandler<UpdatePermissionCommand, Result<PermissionDto>>
{
    private readonly IPermissionRepository _permissionRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;
    private readonly IMediator _mediator;

    public UpdatePermissionCommandHandler(
        IPermissionRepository permissionRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext,
        IMediator mediator)
    {
        _permissionRepository = permissionRepository;
        _unitOfWork = unitOfWork;
        _userContext = userContext;
        _mediator = mediator;
    }

    public async Task<Result<PermissionDto>> Handle(UpdatePermissionCommand request, CancellationToken cancellationToken)
    {
        var permission = await _permissionRepository.GetByIdAsync(request.PermissionId, cancellationToken);

        if (permission is null)
        {
            return Result<PermissionDto>.Fail(new Error(
                "Permission.NotFound",
                $"Permission with Id '{request.PermissionId}' not found",
                ErrorType.NotFound));
        }

        permission.Update(permission.Code, request.Name, request.Description);

        _permissionRepository.Update(permission);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var result = await _mediator.Send(new GetPermissionByIdQuery(permission.PermissionId), cancellationToken);

        return Result<PermissionDto>.Success(result.Value!);
    }
}
