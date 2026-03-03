using Asp.Versioning;
using BSourceCore.API.Contracts.Requests.Auth;
using BSourceCore.API.Contracts.Responses;
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
    [ProducesResponseType(typeof(ApiResponse<TokenResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        _logger.LogInformation("Login attempt for email: {Email}", request.Email);

        var command = new LoginCommand(request.Email, request.Password, request.TenantId);

        try
        {
            var result = await _mediator.Send(command);

            var response = new TokenResponse(
                result.AccessToken,
                result.RefreshToken,
                result.ExpiresAt,
                result.UserId,
                result.Email,
                result.Name,
                result.Permissions,
                result.RequiresPasswordReset,
                result.PasswordResetToken);

            _logger.LogInformation("Login successful for user: {UserId}", result.UserId);

            return Ok(ApiResponse<TokenResponse>.From(response));
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning("Login failed for email: {Email} - {Message}", request.Email, ex.Message);
            return Unauthorized(ApiErrorResponse.Unauthorized(ex.Message));
        }
    }

    /// <summary>
    /// Refreshes an access token using a refresh token
    /// </summary>
    [HttpPost("refresh")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<TokenResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
    {
        _logger.LogInformation("Token refresh attempt");

        var command = new RefreshTokenCommand(request.RefreshToken);

        try
        {
            var result = await _mediator.Send(command);

            var response = new TokenResponse(
                result.AccessToken,
                result.RefreshToken,
                result.ExpiresAt,
                result.UserId,
                result.Email,
                result.Name,
                result.Permissions);

            return Ok(ApiResponse<TokenResponse>.From(response));
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning("Token refresh failed: {Message}", ex.Message);
            return Unauthorized(ApiErrorResponse.Unauthorized(ex.Message));
        }
    }
}
