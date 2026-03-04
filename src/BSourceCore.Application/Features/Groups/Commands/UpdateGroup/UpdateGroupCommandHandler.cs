using BSourceCore.Application.Abstractions;
using BSourceCore.Application.Abstractions.Repositories;
using BSourceCore.Application.Features.Groups.DTOs;
using BSourceCore.Application.Features.Groups.Queries.GetGroupById;
using BSourceCore.Shared.Abstractions;
using BSourceCore.Shared.Kernel.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BSourceCore.Application.Features.Groups.Commands.UpdateGroup;

public class UpdateGroupCommandHandler : IRequestHandler<UpdateGroupCommand, Result<GroupDto>>
{
    private readonly IGroupRepository _groupRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;
    private readonly IMediator _mediator;
    private readonly ILogger<UpdateGroupCommandHandler> _logger;

    public UpdateGroupCommandHandler(
        IGroupRepository groupRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext,
        IMediator mediator,
        ILogger<UpdateGroupCommandHandler> logger)
    {
        _groupRepository = groupRepository;
        _unitOfWork = unitOfWork;
        _userContext = userContext;
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<Result<GroupDto>> Handle(
        UpdateGroupCommand request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating group: {GroupId}", request.GroupId);

        var group = await _groupRepository.GetByIdAsync(request.GroupId, cancellationToken);

        if (group is null)
        {
            _logger.LogWarning("Group not found with Id: {GroupId}", request.GroupId);
            return Result<GroupDto>.Fail(new Error(
                "Group.NotFound",
                $"Group with Id '{request.GroupId}' not found",
                ErrorType.NotFound));
        }

        group.Update(request.Name, request.Description);

        _groupRepository.Update(group);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Group updated: {GroupId}", group.GroupId);

        var result = await _mediator.Send(new GetGroupByIdQuery(group.GroupId), cancellationToken);

        return Result<GroupDto>.Success(result.Value!);
    }
}
