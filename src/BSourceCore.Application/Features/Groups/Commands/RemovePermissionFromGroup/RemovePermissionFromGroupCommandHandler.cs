using BSourceCore.Application.Abstractions;
using BSourceCore.Application.Abstractions.Repositories;
using BSourceCore.Shared.Abstractions;
using BSourceCore.Shared.Kernel.Results;
using MediatR;

namespace BSourceCore.Application.Features.Groups.Commands.RemovePermissionFromGroup;

public class RemovePermissionFromGroupCommandHandler : IRequestHandler<RemovePermissionFromGroupCommand, Result>
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

    public async Task<Result> Handle(RemovePermissionFromGroupCommand request, CancellationToken cancellationToken)
    {
        var groupPermission = await _groupPermissionRepository.GetAsync(
            request.GroupId,
            request.PermissionId,
            cancellationToken);

        if (groupPermission is null)
        {
            return Result.Fail(new Error(
                "Group.PermissionNotAssigned",
                "Permission is not assigned to this group",
                ErrorType.NotFound));
        }

        _groupPermissionRepository.Delete(groupPermission);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
