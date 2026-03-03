using Asp.Versioning;
using BSourceCore.API.Contracts.Requests.Users;
using BSourceCore.API.Contracts.Responses;
using BSourceCore.Application.Features.Users.Commands.CreateUser;
using BSourceCore.Application.Features.Users.Commands.DeleteUser;
using BSourceCore.Application.Features.Users.Commands.UpdateUser;
using BSourceCore.Application.Features.Users.Queries.GetCurrentUser;
using BSourceCore.Application.Features.Users.Queries.GetUserById;
using BSourceCore.Application.Features.Users.Queries.GetUsers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BSourceCore.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<UsersController> _logger;

    public UsersController(IMediator mediator, ILogger<UsersController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Creates a new user
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<UserResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [Authorize(Policy = "users.create")]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
    {
        _logger.LogInformation("Creating user with email: {Email}", request.Email);

        var command = new CreateUserCommand(
            request.TenantId,
            request.Name,
            request.Email);

        var result = await _mediator.Send(command);

        var response = new UserResponse(
            result.UserId,
            request.TenantId,
            result.Name,
            result.Email,
            "Active",
            DateTimeOffset.UtcNow);

        return CreatedAtAction(
            nameof(GetById),
            new { userId = result.UserId },
            ApiResponse<UserResponse>.From(response));
    }

    /// <summary>
    /// Gets a user by ID
    /// </summary>
    [HttpGet("{userId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<UserResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [Authorize(Policy = "users.read")]
    public async Task<IActionResult> GetById(Guid userId)
    {
        _logger.LogInformation("Getting user by Id: {UserId}", userId);

        var query = new GetUserByIdQuery(userId);
        var result = await _mediator.Send(query);

        if (result is null)
        {
            return NotFound(ApiErrorResponse.NotFound($"User with Id '{userId}' not found"));
        }

        var response = new UserResponse(
            result.UserId,
            result.TenantId,
            result.Name,
            result.Email,
            result.Status,
            result.CreatedAt);

        return Ok(ApiResponse<UserResponse>.From(response));
    }

    /// <summary>
    /// Gets all users by tenant
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<UserResponse>), StatusCodes.Status200OK)]
    [Authorize(Policy = "users.read")]
    public async Task<IActionResult> GetAll([FromQuery] Guid tenantId)
    {
        _logger.LogInformation("Getting all users for tenant: {TenantId}", tenantId);

        var query = new GetUsersQuery(tenantId);
        var result = await _mediator.Send(query);

        var response = result.Select(u => new UserResponse(
            u.UserId,
            u.TenantId,
            u.Name,
            u.Email,
            u.Status,
            u.CreatedAt));

        return Ok(ApiResponse<UserResponse>.From(response));
    }

    /// <summary>
    /// Updates a user
    /// </summary>
    [HttpPut("{userId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<UserResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [Authorize(Policy = "users.update")]
    public async Task<IActionResult> Update(Guid userId, [FromBody] UpdateUserRequest request)
    {
        _logger.LogInformation("Updating user: {UserId}", userId);

        var command = new UpdateUserCommand(userId, request.Name, request.Email);
        var result = await _mediator.Send(command);

        var response = new UserResponse(
            result.UserId,
            Guid.Empty, // Would come from context
            result.Name,
            result.Email,
            "Active",
            DateTimeOffset.UtcNow);

        return Ok(ApiResponse<UserResponse>.From(response));
    }

    /// <summary>
    /// Deletes a user (soft delete)
    /// </summary>
    [HttpDelete("{userId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [Authorize(Policy = "users.delete")]
    public async Task<IActionResult> Delete(Guid userId)
    {
        _logger.LogInformation("Deleting user: {UserId}", userId);

        var command = new DeleteUserCommand(userId);
        await _mediator.Send(command);

        return NoContent();
    }

    /// <summary>
    /// Gets the current authenticated user's information
    /// </summary>
    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<CurrentUserResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetCurrentUser()
    {
        _logger.LogInformation("Getting current user data");

        var result = await _mediator.Send(new GetCurrentUserQuery());

        if (result is null)
        {
            return Unauthorized(ApiErrorResponse.Unauthorized("User not found"));
        }

        var response = new CurrentUserResponse(
            result.UserId,
            result.Email,
            result.Name,
            result.TenantId,
            result.PermissionCodes,
            result.Notifications.Select(n => new NotificationResponse(
                n.NotificationId,
                n.Title,
                n.Message,
                n.Data,
                n.WasRead,
                n.CreatedAt)));

        return Ok(ApiResponse<CurrentUserResponse>.From(response));
    }
}
