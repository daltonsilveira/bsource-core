using BSourceCore.Application.Abstractions;
using BSourceCore.Domain.Entities;
using BSourceCore.Shared.Abstractions;
using MediatR;

namespace BSourceCore.Application.Features.Groups.Commands.AddPermissionToGroup;

public class AddPermissionToGroupCommandHandler : IRequestHandler<AddPermissionToGroupCommand, Unit>
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

    public async Task<Unit> Handle(AddPermissionToGroupCommand request, CancellationToken cancellationToken)
    {
        var group = await _groupRepository.GetByIdAsync(request.GroupId, cancellationToken);
        if (group is null)
        {
            throw new KeyNotFoundException($"Group with Id '{request.GroupId}' not found");
        }

        var existingAssignment = await _groupPermissionRepository.GetAsync(
            request.GroupId,
            request.PermissionId,
            cancellationToken);

        if (existingAssignment is not null)
        {
            throw new InvalidOperationException("Permission is already assigned to this group");
        }

        var groupPermission = new GroupPermission(request.GroupId, request.PermissionId);

        await _groupPermissionRepository.AddAsync(groupPermission, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
