using BSourceCore.Application.Abstractions;
using BSourceCore.Application.Abstractions.Repositories;
using BSourceCore.Shared.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BSourceCore.Application.Features.Groups.Commands.UpdateGroup;

public class UpdateGroupCommandHandler : IRequestHandler<UpdateGroupCommand, UpdateGroupResult>
{
    private readonly IGroupRepository _groupRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;
    private readonly ILogger<UpdateGroupCommandHandler> _logger;

    public UpdateGroupCommandHandler(
        IGroupRepository groupRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext,
        ILogger<UpdateGroupCommandHandler> logger)
    {
        _groupRepository = groupRepository;
        _unitOfWork = unitOfWork;
        _userContext = userContext;
        _logger = logger;
    }

    public async Task<UpdateGroupResult> Handle(
        UpdateGroupCommand request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating group: {GroupId}", request.GroupId);

        var group = await _groupRepository.GetByIdAsync(request.GroupId, cancellationToken)
            ?? throw new KeyNotFoundException($"Group with Id '{request.GroupId}' not found");

        group.Update(request.Name, request.Description);

        _groupRepository.Update(group);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Group updated: {GroupId}", group.GroupId);

        return new UpdateGroupResult(group.GroupId, group.Name);
    }
}
