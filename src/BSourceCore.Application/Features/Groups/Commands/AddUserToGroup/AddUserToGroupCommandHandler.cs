using BSourceCore.Application.Abstractions;
using BSourceCore.Application.Abstractions.Repositories;
using BSourceCore.Domain.Entities;
using BSourceCore.Shared.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BSourceCore.Application.Features.Groups.Commands.AddUserToGroup;

public class AddUserToGroupCommandHandler : IRequestHandler<AddUserToGroupCommand, bool>
{
    private readonly IUserGroupRepository _userGroupRepository;
    private readonly IGroupRepository _groupRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;
    private readonly ILogger<AddUserToGroupCommandHandler> _logger;

    public AddUserToGroupCommandHandler(
        IUserGroupRepository userGroupRepository,
        IGroupRepository groupRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext,
        ILogger<AddUserToGroupCommandHandler> logger)
    {
        _userGroupRepository = userGroupRepository;
        _groupRepository = groupRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _userContext = userContext;
        _logger = logger;
    }

    public async Task<bool> Handle(
        AddUserToGroupCommand request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Adding user {UserId} to group {GroupId}", request.UserId, request.GroupId);

        var group = await _groupRepository.GetByIdAsync(request.GroupId, cancellationToken)
            ?? throw new KeyNotFoundException($"Group with Id '{request.GroupId}' not found");

        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken)
            ?? throw new KeyNotFoundException($"User with Id '{request.UserId}' not found");

        var existing = await _userGroupRepository.GetAsync(request.UserId, request.GroupId, cancellationToken);
        if (existing is not null)
        {
            _logger.LogWarning("User {UserId} is already in group {GroupId}", request.UserId, request.GroupId);
            throw new InvalidOperationException("User is already a member of this group");
        }

        var userGroup = new UserGroup(request.UserId, request.GroupId);

        await _userGroupRepository.AddAsync(userGroup, cancellationToken);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("User {UserId} added to group {GroupId}", request.UserId, request.GroupId);

        return true;
    }
}
