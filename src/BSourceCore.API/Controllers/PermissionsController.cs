using Asp.Versioning;
using BSourceCore.API.Contracts.Requests.Permissions;
using BSourceCore.API.Contracts.Responses;
using BSourceCore.Application.Features.Permissions.Commands.CreatePermission;
using BSourceCore.Application.Features.Permissions.Commands.UpdatePermission;
using BSourceCore.Application.Features.Permissions.Queries.GetPermissionById;
using BSourceCore.Application.Features.Permissions.Queries.GetPermissions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BSourceCore.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
[Authorize]
public class PermissionsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<PermissionsController> _logger;

    public PermissionsController(IMediator mediator, ILogger<PermissionsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Creates a new permission
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(PagedResponse<PermissionResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [Authorize(Policy = "permissions.create")]
    public async Task<IActionResult> Create([FromBody] CreatePermissionRequest request)
    {
        _logger.LogInformation("Creating permission with code: {Code}", request.Code);

        var command = new CreatePermissionCommand(
            request.Code,
            request.Name,
            request.Description);

        var result = await _mediator.Send(command);

        var response = new PermissionResponse(
            result.PermissionId,
            result.Code,
            result.Name,
            result.Description,
            result.Status,
            result.CreatedAt);

        return CreatedAtAction(
            nameof(GetById),
            new { permissionId = result.PermissionId },
            PagedResponse<PermissionResponse>.From(response));
    }

    /// <summary>
    /// Gets a permission by ID
    /// </summary>
    [HttpGet("{permissionId:guid}")]
    [ProducesResponseType(typeof(PagedResponse<PermissionResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [Authorize(Policy = "permissions.read")]
    public async Task<IActionResult> GetById(Guid permissionId)
    {
        _logger.LogInformation("Getting permission by Id: {PermissionId}", permissionId);

        var query = new GetPermissionByIdQuery(permissionId);
        var result = await _mediator.Send(query);

        if (result is null)
        {
            return NotFound(ApiErrorResponse.NotFound($"Permission with Id '{permissionId}' not found"));
        }

        var response = new PermissionResponse(
            result.PermissionId,
            result.Code,
            result.Name,
            result.Description,
            result.Status,
            result.CreatedAt);

        return Ok(PagedResponse<PermissionResponse>.From(response));
    }

    /// <summary>
    /// Gets all permissions
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResponse<PermissionResponse>), StatusCodes.Status200OK)]
    [Authorize(Policy = "permissions.read")]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("Getting all permissions");

        var query = new GetPermissionsQuery();
        var result = await _mediator.Send(query);

        var response = result.Select(p => new PermissionResponse(
            p.PermissionId,
            p.Code,
            p.Name,
            p.Description,
            p.Status,
            p.CreatedAt));

        return Ok(PagedResponse<PermissionResponse>.From(response));
    }

    /// <summary>
    /// Updates a permission
    /// </summary>
    [HttpPut("{permissionId:guid}")]
    [ProducesResponseType(typeof(PagedResponse<PermissionResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [Authorize(Policy = "permissions.update")]
    public async Task<IActionResult> Update(Guid permissionId, [FromBody] UpdatePermissionRequest request)
    {
        _logger.LogInformation("Updating permission: {PermissionId}", permissionId);

        var command = new UpdatePermissionCommand(permissionId, request.Name, request.Description);
        var result = await _mediator.Send(command);

        var response = new PermissionResponse(
            result.PermissionId,
            result.Code,
            result.Name,
            result.Description,
            result.Status,
            result.CreatedAt);

        return Ok(PagedResponse<PermissionResponse>.From(response));
    }
}
