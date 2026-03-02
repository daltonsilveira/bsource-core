using BSourceCore.Application.Abstractions;
using BSourceCore.Application.Abstractions.Repositories;
using BSourceCore.Application.Features.Groups.DTOs;
using BSourceCore.Shared.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BSourceCore.Application.Features.Groups.Queries.GetGroupById;

public class GetGroupByIdQueryHandler : IRequestHandler<GetGroupByIdQuery, GroupDto?>
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

    public async Task<GroupDto?> Handle(
        GetGroupByIdQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting group by Id: {GroupId}", request.GroupId);

        var group = await _groupRepository.GetByIdAsync(request.GroupId, cancellationToken);

        if (group is null)
        {
            _logger.LogWarning("Group not found with Id: {GroupId}", request.GroupId);
            return null;
        }

        return new GroupDto(
            group.GroupId,
            group.TenantId,
            group.Name,
            group.Description,
            group.Status.ToString(),
            group.CreatedAt);
    }
}
