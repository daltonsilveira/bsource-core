using Asp.Versioning;
using BSourceCore.API.Contracts.Responses;
using BSourceCore.Application.Features.Notifications.Commands.DeleteRecipient;
using BSourceCore.Application.Features.Notifications.Commands.UpdateRecipientWasRead;
using BSourceCore.Application.Features.Notifications.Queries.GetNotificationById;
using BSourceCore.Application.Features.Notifications.Queries.GetNotifications;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BSourceCore.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
[Authorize]
public class NotificationsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<NotificationsController> _logger;

    public NotificationsController(IMediator mediator, ILogger<NotificationsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Gets all notifications for the authenticated user
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<NotificationResponse>>), StatusCodes.Status200OK)]
    [Authorize(Policy = "notifications.read")]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("Getting all notifications for authenticated user");

        var query = new GetNotificationsQuery();
        var result = await _mediator.Send(query);

        var response = result.Select(n => new NotificationResponse(
            n.NotificationId,
            n.Title,
            n.Message,
            n.Data,
            n.WasRead,
            n.CreatedAt));

        return Ok(ApiResponse<NotificationResponse>.From(response));
    }

    /// <summary>
    /// Gets a notification by ID
    /// </summary>
    [HttpGet("{notificationId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<NotificationResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [Authorize(Policy = "notifications.read")]
    public async Task<IActionResult> GetById(Guid notificationId)
    {
        _logger.LogInformation("Getting notification by Id: {NotificationId}", notificationId);

        var query = new GetNotificationByIdQuery(notificationId);
        var result = await _mediator.Send(query);

        if (result is null)
        {
            return NotFound(ApiErrorResponse.NotFound($"Notification with Id '{notificationId}' not found"));
        }

        var response = new NotificationResponse(
            result.NotificationId,
            result.Title,
            result.Message,
            result.Data,
            result.WasRead,
            result.CreatedAt);

        return Ok(ApiResponse<NotificationResponse>.From(response));
    }

    /// <summary>
    /// Marks notifications as read for the authenticated user.
    /// If notificationId is provided, marks only that notification as read.
    /// If notificationId is null, marks all notifications as read.
    /// </summary>
    [HttpPatch("read")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Authorize(Policy = "notifications.update")]
    public async Task<IActionResult> MarkAsRead([FromQuery] Guid? notificationId)
    {
        _logger.LogInformation("Marking notifications as read. NotificationId: {NotificationId}", notificationId);

        var command = new UpdateRecipientWasReadCommand(notificationId);
        await _mediator.Send(command);

        return Ok();
    }

    /// <summary>
    /// Deletes a notification recipient (removes notification for the authenticated user)
    /// </summary>
    [HttpDelete("recipients/{notificationRecipientId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [Authorize(Policy = "notifications.delete")]
    public async Task<IActionResult> DeleteRecipient(Guid notificationRecipientId)
    {
        _logger.LogInformation("Deleting notification recipient: {NotificationRecipientId}", notificationRecipientId);

        var command = new DeleteRecipientCommand(notificationRecipientId);
        await _mediator.Send(command);

        return Ok();
    }
}
