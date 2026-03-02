using BSourceCore.Application.Abstractions;
using BSourceCore.Application.Abstractions.Repositories;
using BSourceCore.Application.Features.Permissions.DTOs;
using BSourceCore.Domain.Entities;
using BSourceCore.Shared.Abstractions;
using MediatR;

namespace BSourceCore.Application.Features.Permissions.Commands.CreatePermission;

public class CreatePermissionCommandHandler : IRequestHandler<CreatePermissionCommand, PermissionDto>
{
    private readonly IPermissionRepository _permissionRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;

    public CreatePermissionCommandHandler(
        IPermissionRepository permissionRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext)
    {
        _permissionRepository = permissionRepository;
        _unitOfWork = unitOfWork;
        _userContext = userContext;
    }

    public async Task<PermissionDto> Handle(CreatePermissionCommand request, CancellationToken cancellationToken)
    {
        var existingPermission = await _permissionRepository.GetByCodeAsync(request.Code, cancellationToken);
        if (existingPermission is not null)
        {
            throw new InvalidOperationException($"Permission with code '{request.Code}' already exists");
        }

        // Note: Permission requires a TenantId, but this is a global permission.
        // In a real scenario, you might have system-level permissions without tenant
        // For now, we'll need to update the Command to include TenantId
        var permission = new Permission(
            Guid.Empty, // System-level permission
            request.Code,
            request.Name,
            request.Description);

        await _permissionRepository.AddAsync(permission, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new PermissionDto(
            permission.PermissionId,
            permission.Code,
            permission.Name,
            permission.Description,
            permission.Status.ToString(),
            permission.CreatedAt);
    }
}
