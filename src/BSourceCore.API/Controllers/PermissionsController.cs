using Asp.Versioning;
using BSourceCore.API.Contracts.Requests.Permissions;
using BSourceCore.API.Contracts.Responses;
using BSourceCore.API.Extensions;
using BSourceCore.Application.Features.Permissions.Commands.CreatePermission;
using BSourceCore.Application.Features.Permissions.Commands.UpdatePermission;
using BSourceCore.Application.Features.Permissions.DTOs;
using BSourceCore.Application.Features.Permissions.Queries.GetPermissionById;
using BSourceCore.Application.Features.Permissions.Queries.ListPermissions;
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
    [ProducesResponseType(typeof(CollectionResponse<PermissionResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [Authorize(Policy = "permissions.create")]
    public async Task<IActionResult> Create([FromBody] CreatePermissionRequest request)
    {
        _logger.LogInformation("Creating permission with code: {Code}", request.Code);

        var command = new CreatePermissionCommand(
            request.Code,
            request.Name,
            request.Description);

        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
        {
            return result.ToProblemDetails(this);
        }

        return Ok(CollectionResponse<PermissionResponse>.From(ToDefaultResponse(result.Value!)));
    }

    /// <summary>
    /// Gets a permission by ID
    /// </summary>
    [HttpGet("{permissionId:guid}")]
    [ProducesResponseType(typeof(CollectionResponse<PermissionResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [Authorize(Policy = "permissions.read")]
    public async Task<IActionResult> GetById(Guid permissionId)
    {
        _logger.LogInformation("Getting permission by Id: {PermissionId}", permissionId);

        var query = new GetPermissionByIdQuery(permissionId);
        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
        {
            return result.ToProblemDetails(this);
        }

        return Ok(CollectionResponse<PermissionResponse>.From(ToDefaultResponse(result.Value!)));
    }

    /// <summary>
    /// List all permissions
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(CollectionResponse<PermissionResponse>), StatusCodes.Status200OK)]
    [Authorize(Policy = "permissions.read")]
    public async Task<IActionResult> List()
    {
        _logger.LogInformation("Listing all permissions");

        var query = new ListPermissionsQuery();
        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
        {
            return result.ToProblemDetails(this);
        }

        return Ok(CollectionResponse<PermissionResponse>.From(result.Value!.Results.Select(x => ToDefaultResponse(x))));
    }

    /// <summary>
    /// Updates a permission
    /// </summary>
    [HttpPut("{permissionId:guid}")]
    [ProducesResponseType(typeof(CollectionResponse<PermissionResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [Authorize(Policy = "permissions.update")]
    public async Task<IActionResult> Update(Guid permissionId, [FromBody] UpdatePermissionRequest request)
    {
        _logger.LogInformation("Updating permission: {PermissionId}", permissionId);

        var command = new UpdatePermissionCommand(permissionId, request.Name, request.Description);
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
        {
            return result.ToProblemDetails(this);
        }

        return Ok(CollectionResponse<PermissionResponse>.From(ToDefaultResponse(result.Value!)));
    }

    private static PermissionResponse ToDefaultResponse(PermissionDto dto)
    {
        return new PermissionResponse(
            dto.PermissionId,
            dto.Code,
            dto.Name,
            dto.Description,
            dto.Status,
            dto.CreatedAt);
    }
}
