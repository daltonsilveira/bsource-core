using Asp.Versioning;
using BSourceCore.API.Contracts.Requests.Users;
using BSourceCore.API.Contracts.Responses;
using BSourceCore.API.Extensions;
using BSourceCore.Application.Features.Users.Commands.CreateUser;
using BSourceCore.Application.Features.Users.Commands.DeleteUser;
using BSourceCore.Application.Features.Users.Commands.UpdateUser;
using BSourceCore.Application.Features.Users.Queries.GetCurrentUser;
using BSourceCore.Application.Features.Users.Queries.GetUserById;
using BSourceCore.Application.Features.Users.Queries.ListUsers;
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
    [ProducesResponseType(typeof(CollectionResponse<UserResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [Authorize(Policy = "users.create")]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
    {
        _logger.LogInformation("Creating user with email: {Email}", request.Email);

        var command = new CreateUserCommand(
            request.TenantId,
            request.Name,
            request.Email);

        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
        {
            return result.ToProblemDetails(this);
        }

        return Ok(CollectionResponse<UserResponse>.From(new UserResponse(result.Value!)));
    }

    /// <summary>
    /// Gets a user by ID
    /// </summary>
    [HttpGet("{userId:guid}")]
    [ProducesResponseType(typeof(CollectionResponse<UserResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [Authorize(Policy = "users.read")]
    public async Task<IActionResult> GetById(Guid userId)
    {
        _logger.LogInformation("Getting user by Id: {UserId}", userId);

        var query = new GetUserByIdQuery(userId);

        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
        {
            return result.ToProblemDetails(this);
        }

        return Ok(CollectionResponse<UserResponse>.From(new UserResponse(result.Value!)));
    }

    /// <summary>
    /// List users
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(CollectionResponse<UserResponse>), StatusCodes.Status200OK)]
    [Authorize(Policy = "users.read")]
    public async Task<IActionResult> List()
    {
        _logger.LogInformation("Listing all users");

        var query = new ListUsersQuery();
        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
        {
            return result.ToProblemDetails(this);
        }

        return Ok(CollectionResponse<UserResponse>.From(result.Value!.Results.Select(x => new UserResponse(x))));
    }

    /// <summary>
    /// Updates a user
    /// </summary>
    [HttpPut("{userId:guid}")]
    [ProducesResponseType(typeof(CollectionResponse<UserResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [Authorize(Policy = "users.update")]
    public async Task<IActionResult> Update(Guid userId, [FromBody] UpdateUserRequest request)
    {
        _logger.LogInformation("Updating user: {UserId}", userId);

        var command = new UpdateUserCommand(userId, request.Name, request.Email);
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
        {
            return result.ToProblemDetails(this);
        }

        return Ok(CollectionResponse<UserResponse>.From(new UserResponse(result.Value!)));
    }

    /// <summary>
    /// Deletes a user (soft delete)
    /// </summary>
    [HttpDelete("{userId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [Authorize(Policy = "users.delete")]
    public async Task<IActionResult> Delete(Guid userId)
    {
        _logger.LogInformation("Deleting user: {UserId}", userId);

        var command = new DeleteUserCommand(userId);
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
        {
            return result.ToProblemDetails(this);
        }

        return NoContent();
    }

    /// <summary>
    /// Gets the current authenticated user's information
    /// </summary>
    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(typeof(CollectionResponse<CurrentUserResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetCurrentUser()
    {
        _logger.LogInformation("Getting current user data");

        var result = await _mediator.Send(new GetCurrentUserQuery());

        if (!result.IsSuccess)
        {
            return result.ToProblemDetails(this);
        }

        return Ok(CollectionResponse<CurrentUserResponse>.From(new CurrentUserResponse(result.Value!)));
    }
}
