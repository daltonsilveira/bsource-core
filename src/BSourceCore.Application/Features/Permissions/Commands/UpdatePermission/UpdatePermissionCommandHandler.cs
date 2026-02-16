using BSourceCore.Application.Abstractions;
using BSourceCore.Application.Features.Permissions.DTOs;
using BSourceCore.Shared.Abstractions;
using MediatR;

namespace BSourceCore.Application.Features.Permissions.Commands.UpdatePermission;

public class UpdatePermissionCommandHandler : IRequestHandler<UpdatePermissionCommand, PermissionDto>
{
    private readonly IPermissionRepository _permissionRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;

    public UpdatePermissionCommandHandler(
        IPermissionRepository permissionRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext)
    {
        _permissionRepository = permissionRepository;
        _unitOfWork = unitOfWork;
        _userContext = userContext;
    }

    public async Task<PermissionDto> Handle(UpdatePermissionCommand request, CancellationToken cancellationToken)
    {
        var permission = await _permissionRepository.GetByIdAsync(request.PermissionId, cancellationToken);
        if (permission is null)
        {
            throw new KeyNotFoundException($"Permission with Id '{request.PermissionId}' not found");
        }

        permission.Update(permission.Code, request.Name, request.Description);

        _permissionRepository.Update(permission);

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
