using Asp.Versioning;
using BSourceCore.API.Contracts.Requests.PasswordResets;
using BSourceCore.API.Contracts.Responses;
using BSourceCore.Application.Features.PasswordResets.Commands.ConfirmPasswordReset;
using BSourceCore.API.Extensions;
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
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Confirm([FromBody] ConfirmPasswordResetRequest request)
    {
        _logger.LogInformation("Password reset confirmation attempt");

        var command = new ConfirmPasswordResetCommand(request.Token, request.Password);

        var result = await _mediator.Send(command);

        if (!result.IsSuccess) return result.ToProblemDetails(this);

        _logger.LogInformation("Password reset confirmed successfully");

        return NoContent();
    }
}
