using BSourceCore.Application.Abstractions;
using BSourceCore.Application.Abstractions.Repositories;
using BSourceCore.Application.Features.Groups.DTOs;
using BSourceCore.Application.Features.Users.DTOs;
using BSourceCore.Shared.Abstractions;
using BSourceCore.Shared.Kernel.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BSourceCore.Application.Features.Groups.Queries.GetGroupById;

public class GetGroupByIdQueryHandler : IRequestHandler<GetGroupByIdQuery, Result<GroupDto>>
{
    private readonly IGroupRepository _groupRepository;
    private readonly IUserContext _userContext;
    private readonly ILogger<GetGroupByIdQueryHandler> _logger;

    public GetGroupByIdQueryHandler(
        IGroupRepository groupRepository,
        IUserContext userContext,
        ILogger<GetGroupByIdQueryHandler> logger)
    {
        _groupRepository = groupRepository;
        _userContext = userContext;
        _logger = logger;
    }

    public async Task<Result<GroupDto>> Handle(
        GetGroupByIdQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting group by Id: {GroupId}", request.GroupId);

        var group = await _groupRepository.GetByIdAsync(request.GroupId, cancellationToken);

        if (group is null)
        {
            _logger.LogWarning("Group not found with Id: {GroupId}", request.GroupId);
            return Result<GroupDto>.Fail(new Error(
                "Group.NotFound",
                $"Group with Id '{request.GroupId}' not found",
                ErrorType.NotFound));
        }

        return Result<GroupDto>.Success(new GroupDto(
            group.GroupId,
            group.TenantId,
            group.Name,
            group.Description,
            group.Status,
            group.CreatedAt,
            group.CreatedBy != null ? new UserAuditDto(group.CreatedBy.UserId, group.CreatedBy.Name) : null,
            group.UpdatedAt,
            group.UpdatedBy != null ? new UserAuditDto(group.UpdatedBy.UserId, group.UpdatedBy.Name) : null));
    }
}
