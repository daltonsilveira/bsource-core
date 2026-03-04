using BSourceCore.Application.Abstractions;
using BSourceCore.Application.Abstractions.Repositories;
using BSourceCore.Application.Features.Groups.DTOs;
using BSourceCore.Application.Features.Groups.Queries.GetGroupById;
using BSourceCore.Domain.Entities;
using BSourceCore.Shared.Abstractions;
using BSourceCore.Shared.Kernel.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BSourceCore.Application.Features.Groups.Commands.CreateGroup;

public class CreateGroupCommandHandler : IRequestHandler<CreateGroupCommand, Result<GroupDto>>
{
    private readonly IGroupRepository _groupRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;
    private readonly IMediator _mediator;
    private readonly ILogger<CreateGroupCommandHandler> _logger;

    public CreateGroupCommandHandler(
        IGroupRepository groupRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext,
        IMediator mediator,
        ILogger<CreateGroupCommandHandler> logger)
    {
        _groupRepository = groupRepository;
        _unitOfWork = unitOfWork;
        _userContext = userContext;
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<Result<GroupDto>> Handle(
        CreateGroupCommand request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating group with name: {Name}", request.Name);

        var existingGroup = await _groupRepository.GetByNameAsync(request.TenantId, request.Name, cancellationToken);
        if (existingGroup is not null)
        {
            _logger.LogWarning("Group with name {Name} already exists in tenant {TenantId}", request.Name, request.TenantId);
            return Result<GroupDto>.Fail(new Error(
                "Group.NameConflict",
                $"Group with name '{request.Name}' already exists",
                ErrorType.Conflict));
        }

        var group = new Group(request.TenantId, request.Name, request.Description);

        await _groupRepository.AddAsync(group, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Group created with Id: {GroupId}", group.GroupId);

        var result = await _mediator.Send(new GetGroupByIdQuery(group.GroupId), cancellationToken);

        return Result<GroupDto>.Success(result.Value!);
    }
}
