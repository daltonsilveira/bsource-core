using BSourceCore.Application.Abstractions;
using BSourceCore.Application.Abstractions.Repositories;
using BSourceCore.Domain.Enums;
using BSourceCore.Shared.Abstractions;
using BSourceCore.Shared.Kernel.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BSourceCore.Application.Features.Groups.Commands.DeleteGroup;

public class DeleteGroupCommandHandler : IRequestHandler<DeleteGroupCommand, Result>
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
        _unitOfWork = unitOfWork;
        _userContext = userContext;
        _logger = logger;
    }

    public async Task<Result> Handle(
        DeleteGroupCommand request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting group: {GroupId}", request.GroupId);

        var group = await _groupRepository.GetByIdAsync(request.GroupId, cancellationToken);

        if (group is null)
        {
            _logger.LogWarning("Group not found with Id: {GroupId}", request.GroupId);
            return Result.Fail(new Error(
                "Group.NotFound",
                $"Group with Id '{request.GroupId}' not found",
                ErrorType.NotFound));
        }

        group.SetStatus(BaseStatus.Deleted);

        _groupRepository.Update(group);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Group deleted: {GroupId}", group.GroupId);

        return Result.Success();
    }
}
