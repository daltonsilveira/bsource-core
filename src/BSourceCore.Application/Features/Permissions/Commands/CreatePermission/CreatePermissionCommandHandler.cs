using BSourceCore.Application.Abstractions;
using BSourceCore.Application.Abstractions.Repositories;
using BSourceCore.Application.Features.Permissions.DTOs;
using BSourceCore.Application.Features.Permissions.Queries.GetPermissionById;
using BSourceCore.Domain.Entities;
using BSourceCore.Shared.Abstractions;
using BSourceCore.Shared.Kernel.Results;
using MediatR;

namespace BSourceCore.Application.Features.Permissions.Commands.CreatePermission;

public class CreatePermissionCommandHandler : IRequestHandler<CreatePermissionCommand, Result<PermissionDto>>
{
    private readonly IPermissionRepository _permissionRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;
    private readonly IMediator _mediator;

    public CreatePermissionCommandHandler(
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

    public async Task<Result<PermissionDto>> Handle(CreatePermissionCommand request, CancellationToken cancellationToken)
    {
        var existingPermission = await _permissionRepository.GetByCodeAsync(request.Code, cancellationToken);
        if (existingPermission is not null)
        {
            return Result<PermissionDto>.Fail(new Error(
                "Permission.CodeConflict",
                $"Permission with code '{request.Code}' already exists",
                ErrorType.Conflict));
        }

        var permission = new Permission(
            Guid.Empty,
            request.Code,
            request.Name,
            request.Description);

        await _permissionRepository.AddAsync(permission, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var result = await _mediator.Send(new GetPermissionByIdQuery(permission.PermissionId), cancellationToken);

        return Result<PermissionDto>.Success(result.Value!);
    }
}
