using BSourceCore.Application.Abstractions;
using BSourceCore.Application.Abstractions.Repositories;
using BSourceCore.Application.Features.Groups.DTOs;
using BSourceCore.Application.Features.Users.DTOs;
using BSourceCore.Shared.Abstractions;
using BSourceCore.Shared.Kernel.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BSourceCore.Application.Features.Groups.Queries.ListGroups;

public class ListGroupsQueryHandler : IRequestHandler<ListGroupsQuery, Result<CollectionResult<GroupDto>>>
{
    private readonly IGroupRepository _groupRepository;
    private readonly IUserContext _userContext;
    private readonly ILogger<ListGroupsQueryHandler> _logger;

    public ListGroupsQueryHandler(
        IGroupRepository groupRepository,
        IUserContext userContext,
        ILogger<ListGroupsQueryHandler> logger)
    {
        _groupRepository = groupRepository;
        _userContext = userContext;
        _logger = logger;
    }

    public async Task<Result<CollectionResult<GroupDto>>> Handle(
        ListGroupsQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting groups for tenant: {TenantId}", request.TenantId);

        var groups = await _groupRepository.GetAllByTenantAsync(request.TenantId, cancellationToken);

        var items = groups.Select(g => new GroupDto(
            g.GroupId,
            g.TenantId,
            g.Name,
            g.Description,
            g.Status,
            g.CreatedAt,
            g.CreatedBy != null ? new UserAuditDto(g.CreatedBy.UserId, g.CreatedBy.Name) : null,
            g.UpdatedAt,
            g.UpdatedBy != null ? new UserAuditDto(g.UpdatedBy.UserId, g.UpdatedBy.Name) : null)).ToList();

        return Result<CollectionResult<GroupDto>>.Success(CollectionResult<GroupDto>.From(items));
    }
}
