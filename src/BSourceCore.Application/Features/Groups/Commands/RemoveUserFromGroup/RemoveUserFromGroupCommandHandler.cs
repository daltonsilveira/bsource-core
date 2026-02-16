using BSourceCore.Application.Abstractions;
using BSourceCore.Shared.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BSourceCore.Application.Features.Groups.Commands.RemoveUserFromGroup;

public class RemoveUserFromGroupCommandHandler : IRequestHandler<RemoveUserFromGroupCommand, bool>
{
    private readonly IUserGroupRepository _userGroupRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;
    private readonly ILogger<RemoveUserFromGroupCommandHandler> _logger;

    public RemoveUserFromGroupCommandHandler(
        IUserGroupRepository userGroupRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext,
        ILogger<RemoveUserFromGroupCommandHandler> logger)
    {
        _userGroupRepository = userGroupRepository;
        _unitOfWork = unitOfWork;
        _userContext = userContext;
        _logger = logger;
    }

    public async Task<bool> Handle(
        RemoveUserFromGroupCommand request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Removing user {UserId} from group {GroupId}", request.UserId, request.GroupId);

        var userGroup = await _userGroupRepository.GetAsync(request.UserId, request.GroupId, cancellationToken)
            ?? throw new KeyNotFoundException($"User is not a member of this group");

        _userGroupRepository.Delete(userGroup);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("User {UserId} removed from group {GroupId}", request.UserId, request.GroupId);

        return true;
    }
}
