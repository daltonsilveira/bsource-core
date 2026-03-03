using Asp.Versioning;
using BSourceCore.API.Contracts.Requests.Auth;
using BSourceCore.API.Contracts.Responses;
using BSourceCore.API.Extensions;
using BSourceCore.Application.Features.Auth.Commands.Login;
using BSourceCore.Application.Features.Auth.Commands.RefreshToken;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BSourceCore.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IMediator mediator, ILogger<AuthController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Authenticates a user and returns access tokens
    /// </summary>
    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(CollectionResponse<TokenResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        _logger.LogInformation("Login attempt for email: {Email}", request.Email);

        var command = new LoginCommand(request.Email, request.Password, request.TenantId);
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
        {
            return result.ToProblemDetails(this);
        }

        return Ok(CollectionResponse<TokenResponse>.From(new TokenResponse(
                result.Value!.AccessToken,
                result.Value!.RefreshToken,
                result.Value!.ExpiresAt,
                result.Value!.UserId,
                result.Value!.Email,
                result.Value!.Name,
                result.Value!.RequiresPasswordReset,
                result.Value!.PasswordResetToken)));
    }

    /// <summary>
    /// Refreshes an access token using a refresh token
    /// </summary>
    [HttpPost("refresh")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(CollectionResponse<TokenResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
    {
        _logger.LogInformation("Token refresh attempt");

        var command = new RefreshTokenCommand(request.RefreshToken);

        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
        {
            return result.ToProblemDetails(this);
        }

        var response = new TokenResponse(
            result.Value!.AccessToken,
            result.Value!.RefreshToken,
            result.Value!.ExpiresAt,
            result.Value!.UserId,
            result.Value!.Email,
            result.Value!.Name);

        return Ok(CollectionResponse<TokenResponse>.From(response));
    }
}
