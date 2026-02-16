using BSourceCore.Application.Abstractions;
using BSourceCore.Domain.Enums;
using BSourceCore.Shared.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BSourceCore.Application.Features.Groups.Commands.DeleteGroup;

public class DeleteGroupCommandHandler : IRequestHandler<DeleteGroupCommand, bool>
{
    private readonly IGroupRepository _groupRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;
    private readonly ILogger<DeleteGroupCommandHandler> _logger;

    public DeleteGroupCommandHandler(
        IGroupRepository groupRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext,
        ILogger<DeleteGroupCommandHandler> logger)
    {
        _groupRepository = groupRepository;
        _userContext = userContext;
        _logger = logger;
    }

    public async Task<bool> Handle(
        DeleteGroupCommand request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting group: {GroupId}", request.GroupId);

        var group = await _groupRepository.GetByIdAsync(request.GroupId, cancellationToken)
            ?? throw new KeyNotFoundException($"Group with Id '{request.GroupId}' not found");

        group.SetStatus(BaseStatus.Deleted);

        _groupRepository.Update(group);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Group deleted: {GroupId}", group.GroupId);

        return true;
    }
}
