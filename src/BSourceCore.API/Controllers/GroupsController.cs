using Asp.Versioning;
using BSourceCore.API.Contracts.Requests.Groups;
using BSourceCore.API.Contracts.Responses;
using BSourceCore.API.Extensions;
using BSourceCore.Application.Features.Groups.Commands.AddPermissionToGroup;
using BSourceCore.Application.Features.Groups.Commands.AddUserToGroup;
using BSourceCore.Application.Features.Groups.Commands.CreateGroup;
using BSourceCore.Application.Features.Groups.Commands.DeleteGroup;
using BSourceCore.Application.Features.Groups.Commands.RemovePermissionFromGroup;
using BSourceCore.Application.Features.Groups.Commands.RemoveUserFromGroup;
using BSourceCore.Application.Features.Groups.Commands.UpdateGroup;
using BSourceCore.Application.Features.Groups.DTOs;
using BSourceCore.Application.Features.Groups.Queries.GetGroupById;
using BSourceCore.Application.Features.Groups.Queries.ListGroups;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BSourceCore.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
[Authorize]
public class GroupsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<GroupsController> _logger;

    public GroupsController(IMediator mediator, ILogger<GroupsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Creates a new group
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(CollectionResponse<GroupResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [Authorize(Policy = "groups.create")]
    public async Task<IActionResult> Create([FromBody] CreateGroupRequest request)
    {
        _logger.LogInformation("Creating group with name: {Name}", request.Name);

        var result = await _mediator.Send(new CreateGroupCommand(
            request.TenantId,
            request.Name,
            request.Description));

        if (!result.IsSuccess) return result.ToProblemDetails(this);

        return Ok(CollectionResponse<GroupResponse>.From(ToDefaultResponse(result.Value!)));
    }

    /// <summary>
    /// Gets a group by ID
    /// </summary>
    [HttpGet("{groupId:guid}")]
    [ProducesResponseType(typeof(CollectionResponse<GroupResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [Authorize(Policy = "groups.read")]
    public async Task<IActionResult> GetById(Guid groupId)
    {
        _logger.LogInformation("Getting group by Id: {GroupId}", groupId);

        var result = await _mediator.Send(new GetGroupByIdQuery(groupId));

        if (!result.IsSuccess) return result.ToProblemDetails(this);

        return Ok(CollectionResponse<GroupResponse>.From(ToDefaultResponse(result.Value!)));
    }

    /// <summary>
    /// List groups by tenant
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(CollectionResponse<GroupResponse>), StatusCodes.Status200OK)]
    [Authorize(Policy = "groups.read")]
    public async Task<IActionResult> List([FromQuery] Guid tenantId)
    {
        _logger.LogInformation("Listing all groups for tenant: {TenantId}", tenantId);

        var result = await _mediator.Send(new ListGroupsQuery(tenantId));

        if (!result.IsSuccess) return result.ToProblemDetails(this);

        return Ok(CollectionResponse<GroupResponse>.From(result.Value!.Results.Select(x => ToDefaultResponse(x))));
    }

    /// <summary>
    /// Updates a group
    /// </summary>
    [HttpPut("{groupId:guid}")]
    [ProducesResponseType(typeof(CollectionResponse<GroupResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [Authorize(Policy = "groups.update")]
    public async Task<IActionResult> Update(Guid groupId, [FromBody] UpdateGroupRequest request)
    {
        _logger.LogInformation("Updating group: {GroupId}", groupId);

        var result = await _mediator.Send(new UpdateGroupCommand(
            groupId, 
            request.Name, 
            request.Description));

        if (!result.IsSuccess) return result.ToProblemDetails(this);

        return Ok(CollectionResponse<GroupResponse>.From(ToDefaultResponse(result.Value!)));
    }

    /// <summary>
    /// Deletes a group (soft delete)
    /// </summary>
    [HttpDelete("{groupId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [Authorize(Policy = "groups.delete")]
    public async Task<IActionResult> Delete(Guid groupId)
    {
        _logger.LogInformation("Deleting group: {GroupId}", groupId);

        var result = await _mediator.Send(new DeleteGroupCommand(groupId));

        if (!result.IsSuccess) return result.ToProblemDetails(this);

        return NoContent();
    }

    /// <summary>
    /// Adds a user to a group
    /// </summary>
    [HttpPost("{groupId:guid}/users")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [Authorize(Policy = "groups.update")]
    public async Task<IActionResult> AddUser(Guid groupId, [FromBody] AddUserToGroupRequest request)
    {
        _logger.LogInformation("Adding user {UserId} to group {GroupId}", request.UserId, groupId);

        var result = await _mediator.Send(new AddUserToGroupCommand(groupId, request.UserId));

        if (!result.IsSuccess) return result.ToProblemDetails(this);

        return NoContent();
    }

    /// <summary>
    /// Removes a user from a group
    /// </summary>
    [HttpDelete("{groupId:guid}/users/{userId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [Authorize(Policy = "groups.update")]
    public async Task<IActionResult> RemoveUser(Guid groupId, Guid userId)
    {
        _logger.LogInformation("Removing user {UserId} from group {GroupId}", userId, groupId);

        var result = await _mediator.Send(new RemoveUserFromGroupCommand(groupId, userId));

        if (!result.IsSuccess) return result.ToProblemDetails(this);

        return NoContent();
    }

    /// <summary>
    /// Adds a permission to a group
    /// </summary>
    [HttpPost("{groupId:guid}/permissions")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [Authorize(Policy = "groups.update")]
    public async Task<IActionResult> AddPermission(Guid groupId, [FromBody] AddPermissionToGroupRequest request)
    {
        _logger.LogInformation("Adding permission {PermissionId} to group {GroupId}", request.PermissionId, groupId);

        var result = await _mediator.Send(new AddPermissionToGroupCommand(groupId, request.PermissionId));

        if (!result.IsSuccess)  return result.ToProblemDetails(this);

        return NoContent();
    }

    /// <summary>
    /// Removes a permission from a group
    /// </summary>
    [HttpDelete("{groupId:guid}/permissions/{permissionId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [Authorize(Policy = "groups.update")]
    public async Task<IActionResult> RemovePermission(Guid groupId, Guid permissionId)
    {
        _logger.LogInformation("Removing permission {PermissionId} from group {GroupId}", permissionId, groupId);

        var result = await _mediator.Send(new RemovePermissionFromGroupCommand(groupId, permissionId));

        if (!result.IsSuccess)  return result.ToProblemDetails(this);

        return NoContent();
    }

    private static GroupResponse ToDefaultResponse(GroupDto dto)
    {
        return new GroupResponse(
            dto.GroupId,
            dto.TenantId,
            dto.Name,
            dto.Description,
            dto.Status.ToString(),
            dto.CreatedAt,
            dto.CreatedBy != null ? new UserAuditResponse(
                dto.CreatedBy.UserId,
                dto.CreatedBy.Name) : null,
            dto.UpdatedAt,
            dto.UpdatedBy != null ? new UserAuditResponse(
                dto.UpdatedBy.UserId,
                dto.UpdatedBy.Name) : null);
    }
}
