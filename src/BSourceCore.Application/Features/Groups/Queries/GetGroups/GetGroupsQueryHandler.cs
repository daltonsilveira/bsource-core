using BSourceCore.Application.Abstractions;
using BSourceCore.Application.Abstractions.Repositories;
using BSourceCore.Application.Features.Groups.DTOs;
using BSourceCore.Shared.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BSourceCore.Application.Features.Groups.Queries.GetGroups;

public class GetGroupsQueryHandler : IRequestHandler<GetGroupsQuery, IEnumerable<GroupDto>>
{
    private readonly IGroupRepository _groupRepository;
    private readonly IUserContext _userContext;
    private readonly ILogger<GetGroupsQueryHandler> _logger;

    public GetGroupsQueryHandler(
        IGroupRepository groupRepository,
        IUserContext userContext,
        ILogger<GetGroupsQueryHandler> logger)
    {
        _groupRepository = groupRepository;
        _userContext = userContext;
        _logger = logger;
    }

    public async Task<IEnumerable<GroupDto>> Handle(
        GetGroupsQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting groups for tenant: {TenantId}", request.TenantId);

        var groups = await _groupRepository.GetAllByTenantAsync(request.TenantId, cancellationToken);

        return groups.Select(g => new GroupDto(
            g.GroupId,
            g.TenantId,
            g.Name,
            g.Description,
            g.Status.ToString(),
            g.CreatedAt));
    }
}
