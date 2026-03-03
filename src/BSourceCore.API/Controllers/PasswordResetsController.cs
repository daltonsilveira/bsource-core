using Asp.Versioning;
using BSourceCore.API.Contracts.Requests.PasswordResets;
using BSourceCore.API.Contracts.Responses;
using BSourceCore.Application.Features.PasswordResets.Commands.ConfirmPasswordReset;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BSourceCore.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
public class PasswordResetsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<PasswordResetsController> _logger;

    public PasswordResetsController(IMediator mediator, ILogger<PasswordResetsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Confirms a password reset and sets a new password
    /// </summary>
    [HttpPatch("confirm")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Confirm([FromBody] ConfirmPasswordResetRequest request)
    {
        _logger.LogInformation("Password reset confirmation attempt");

        var command = new ConfirmPasswordResetCommand(request.Token, request.Password);

        try
        {
            await _mediator.Send(command);

            _logger.LogInformation("Password reset confirmed successfully");

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Password reset confirmation failed: {Message}", ex.Message);
            return BadRequest(ApiErrorResponse.BadRequest(ex.Message));
        }
    }
}
