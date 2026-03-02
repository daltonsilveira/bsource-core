using BSourceCore.Application.Abstractions;
using BSourceCore.Application.Abstractions.Repositories;
using BSourceCore.Domain.Entities;
using BSourceCore.Shared.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BSourceCore.Application.Features.Groups.Commands.CreateGroup;

public class CreateGroupCommandHandler : IRequestHandler<CreateGroupCommand, CreateGroupResult>
{
    private readonly IGroupRepository _groupRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;
    private readonly ILogger<CreateGroupCommandHandler> _logger;

    public CreateGroupCommandHandler(
        IGroupRepository groupRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext,
        ILogger<CreateGroupCommandHandler> logger)
    {
        _groupRepository = groupRepository;
        _unitOfWork = unitOfWork;
        _userContext = userContext;
        _logger = logger;
    }

    public async Task<CreateGroupResult> Handle(
        CreateGroupCommand request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating group with name: {Name}", request.Name);

        var existingGroup = await _groupRepository.GetByNameAsync(request.TenantId, request.Name, cancellationToken);
        if (existingGroup is not null)
        {
            _logger.LogWarning("Group with name {Name} already exists in tenant {TenantId}", request.Name, request.TenantId);
            throw new InvalidOperationException($"Group with name '{request.Name}' already exists");
        }

        var group = new Group(request.TenantId, request.Name, request.Description);

        await _groupRepository.AddAsync(group, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Group created with Id: {GroupId}", group.GroupId);

        return new CreateGroupResult(group.GroupId, group.Name);
    }
}
