using BSourceCore.Application.Abstractions;
using BSourceCore.Shared.Abstractions;
using MediatR;

namespace BSourceCore.Application.Features.Groups.Commands.RemovePermissionFromGroup;

public class RemovePermissionFromGroupCommandHandler : IRequestHandler<RemovePermissionFromGroupCommand, Unit>
{
    private readonly IGroupPermissionRepository _groupPermissionRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;

    public RemovePermissionFromGroupCommandHandler(
        IGroupPermissionRepository groupPermissionRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext)
    {
        _groupPermissionRepository = groupPermissionRepository;
        _unitOfWork = unitOfWork;
        _userContext = userContext;
    }

    public async Task<Unit> Handle(RemovePermissionFromGroupCommand request, CancellationToken cancellationToken)
    {
        var groupPermission = await _groupPermissionRepository.GetAsync(
            request.GroupId,
            request.PermissionId,
            cancellationToken);

        if (groupPermission is null)
        {
            throw new KeyNotFoundException("Permission is not assigned to this group");
        }

        _groupPermissionRepository.Delete(groupPermission);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
