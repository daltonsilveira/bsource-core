using Asp.Versioning;
using BSourceCore.API.Requests.Groups;
using BSourceCore.API.Responses;
using BSourceCore.Application.Features.Groups.Commands.AddPermissionToGroup;
using BSourceCore.Application.Features.Groups.Commands.AddUserToGroup;
using BSourceCore.Application.Features.Groups.Commands.CreateGroup;
using BSourceCore.Application.Features.Groups.Commands.DeleteGroup;
using BSourceCore.Application.Features.Groups.Commands.RemovePermissionFromGroup;
using BSourceCore.Application.Features.Groups.Commands.RemoveUserFromGroup;
using BSourceCore.Application.Features.Groups.Commands.UpdateGroup;
using BSourceCore.Application.Features.Groups.Queries.GetGroupById;
using BSourceCore.Application.Features.Groups.Queries.GetGroups;
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
    [ProducesResponseType(typeof(ApiResponse<GroupResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [Authorize(Policy = "groups.create")]
    public async Task<IActionResult> Create([FromBody] CreateGroupRequest request)
    {
        _logger.LogInformation("Creating group with name: {Name}", request.Name);

        var command = new CreateGroupCommand(
            request.TenantId,
            request.Name,
            request.Description);

        var result = await _mediator.Send(command);

        var response = new GroupResponse(
            result.GroupId,
            request.TenantId,
            result.Name,
            request.Description,
            "Active",
            DateTimeOffset.UtcNow);

        return CreatedAtAction(
            nameof(GetById),
            new { groupId = result.GroupId },
            ApiResponse<GroupResponse>.Ok(response));
    }

    /// <summary>
    /// Gets a group by ID
    /// </summary>
    [HttpGet("{groupId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<GroupResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [Authorize(Policy = "groups.read")]
    public async Task<IActionResult> GetById(Guid groupId)
    {
        _logger.LogInformation("Getting group by Id: {GroupId}", groupId);

        var query = new GetGroupByIdQuery(groupId);
        var result = await _mediator.Send(query);

        if (result is null)
        {
            return NotFound(ApiResponse.Fail($"Group with Id '{groupId}' not found"));
        }

        var response = new GroupResponse(
            result.GroupId,
            result.TenantId,
            result.Name,
            result.Description,
            result.Status,
            result.CreatedAt);

        return Ok(ApiResponse<GroupResponse>.Ok(response));
    }

    /// <summary>
    /// Gets all groups by tenant
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<GroupResponse>>), StatusCodes.Status200OK)]
    [Authorize(Policy = "groups.read")]
    public async Task<IActionResult> GetAll([FromQuery] Guid tenantId)
    {
        _logger.LogInformation("Getting all groups for tenant: {TenantId}", tenantId);

        var query = new GetGroupsQuery(tenantId);
        var result = await _mediator.Send(query);

        var response = result.Select(g => new GroupResponse(
            g.GroupId,
            g.TenantId,
            g.Name,
            g.Description,
            g.Status,
            g.CreatedAt));

        return Ok(ApiResponse<IEnumerable<GroupResponse>>.Ok(response));
    }

    /// <summary>
    /// Updates a group
    /// </summary>
    [HttpPut("{groupId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<GroupResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [Authorize(Policy = "groups.update")]
    public async Task<IActionResult> Update(Guid groupId, [FromBody] UpdateGroupRequest request)
    {
        _logger.LogInformation("Updating group: {GroupId}", groupId);

        var command = new UpdateGroupCommand(groupId, request.Name, request.Description);
        var result = await _mediator.Send(command);

        var response = new GroupResponse(
            result.GroupId,
            Guid.Empty,
            result.Name,
            request.Description,
            "Active",
            DateTimeOffset.UtcNow);

        return Ok(ApiResponse<GroupResponse>.Ok(response));
    }

    /// <summary>
    /// Deletes a group (soft delete)
    /// </summary>
    [HttpDelete("{groupId:guid}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [Authorize(Policy = "groups.delete")]
    public async Task<IActionResult> Delete(Guid groupId)
    {
        _logger.LogInformation("Deleting group: {GroupId}", groupId);

        var command = new DeleteGroupCommand(groupId);
        await _mediator.Send(command);

        return Ok(ApiResponse.Ok());
    }

    /// <summary>
    /// Adds a user to a group
    /// </summary>
    [HttpPost("{groupId:guid}/users")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [Authorize(Policy = "groups.update")]
    public async Task<IActionResult> AddUser(Guid groupId, [FromBody] AddUserToGroupRequest request)
    {
        _logger.LogInformation("Adding user {UserId} to group {GroupId}", request.UserId, groupId);

        var command = new AddUserToGroupCommand(groupId, request.UserId);
        await _mediator.Send(command);

        return Ok(ApiResponse.Ok());
    }

    /// <summary>
    /// Removes a user from a group
    /// </summary>
    [HttpDelete("{groupId:guid}/users/{userId:guid}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [Authorize(Policy = "groups.update")]
    public async Task<IActionResult> RemoveUser(Guid groupId, Guid userId)
    {
        _logger.LogInformation("Removing user {UserId} from group {GroupId}", userId, groupId);

        var command = new RemoveUserFromGroupCommand(groupId, userId);
        await _mediator.Send(command);

        return Ok(ApiResponse.Ok());
    }

    /// <summary>
    /// Adds a permission to a group
    /// </summary>
    [HttpPost("{groupId:guid}/permissions")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [Authorize(Policy = "groups.update")]
    public async Task<IActionResult> AddPermission(Guid groupId, [FromBody] AddPermissionToGroupRequest request)
    {
        _logger.LogInformation("Adding permission {PermissionId} to group {GroupId}", request.PermissionId, groupId);

        var command = new AddPermissionToGroupCommand(groupId, request.PermissionId);
        await _mediator.Send(command);

        return Ok(ApiResponse.Ok());
    }

    /// <summary>
    /// Removes a permission from a group
    /// </summary>
    [HttpDelete("{groupId:guid}/permissions/{permissionId:guid}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [Authorize(Policy = "groups.update")]
    public async Task<IActionResult> RemovePermission(Guid groupId, Guid permissionId)
    {
        _logger.LogInformation("Removing permission {PermissionId} from group {GroupId}", permissionId, groupId);

        var command = new RemovePermissionFromGroupCommand(groupId, permissionId);
        await _mediator.Send(command);

        return Ok(ApiResponse.Ok());
    }
}
