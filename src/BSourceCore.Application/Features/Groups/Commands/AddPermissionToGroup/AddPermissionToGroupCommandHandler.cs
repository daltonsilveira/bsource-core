using BSourceCore.Application.Abstractions;
using BSourceCore.Application.Abstractions.Repositories;
using BSourceCore.Domain.Entities;
using BSourceCore.Shared.Abstractions;
using BSourceCore.Shared.Kernel.Results;
using MediatR;

namespace BSourceCore.Application.Features.Groups.Commands.AddPermissionToGroup;

public class AddPermissionToGroupCommandHandler : IRequestHandler<AddPermissionToGroupCommand, Result>
{
    private readonly IGroupRepository _groupRepository;
    private readonly IGroupPermissionRepository _groupPermissionRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;

    public AddPermissionToGroupCommandHandler(
        IGroupRepository groupRepository,
        IGroupPermissionRepository groupPermissionRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext)
    {
        _groupRepository = groupRepository;
        _groupPermissionRepository = groupPermissionRepository;
        _unitOfWork = unitOfWork;
        _userContext = userContext;
    }

    public async Task<Result> Handle(AddPermissionToGroupCommand request, CancellationToken cancellationToken)
    {
        var group = await _groupRepository.GetByIdAsync(request.GroupId, cancellationToken);
        if (group is null)
        {
            return Result.Fail(new Error(
                "Group.NotFound",
                $"Group with Id '{request.GroupId}' not found",
                ErrorType.NotFound));
        }

        var existingAssignment = await _groupPermissionRepository.GetAsync(
            request.GroupId,
            request.PermissionId,
            cancellationToken);

        if (existingAssignment is not null)
        {
            return Result.Fail(new Error(
                "Group.PermissionAlreadyAssigned",
                "Permission is already assigned to this group",
                ErrorType.Conflict));
        }

        var groupPermission = new GroupPermission(request.GroupId, request.PermissionId);

        await _groupPermissionRepository.AddAsync(groupPermission, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
