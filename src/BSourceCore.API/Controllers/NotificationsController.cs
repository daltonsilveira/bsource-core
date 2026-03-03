using Asp.Versioning;
using BSourceCore.API.Contracts.Responses;
using BSourceCore.API.Extensions;
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
    [ProducesResponseType(typeof(CollectionResponse<NotificationResponse>), StatusCodes.Status200OK)]
    [Authorize(Policy = "notifications.read")]
    public async Task<IActionResult> List()
    {
        _logger.LogInformation("Listing notifications for authenticated user");

        var query = new GetNotificationsQuery();
        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
        {
            return result.ToProblemDetails(this);
        }

        var response = result.Value!.Results.Select(n => new NotificationResponse(
            n.NotificationId,
            n.Title,
            n.Message,
            n.Data,
            n.WasRead,
            n.CreatedAt));

        return Ok(CollectionResponse<NotificationResponse>.From(response));
    }

    /// <summary>
    /// Gets a notification by ID
    /// </summary>
    [HttpGet("{notificationId:guid}")]
    [ProducesResponseType(typeof(CollectionResponse<NotificationResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [Authorize(Policy = "notifications.read")]
    public async Task<IActionResult> GetById(Guid notificationId)
    {
        _logger.LogInformation("Getting notification by Id: {NotificationId}", notificationId);

        var query = new GetNotificationByIdQuery(notificationId);
        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
        {
            return result.ToProblemDetails(this);
        }

        var response = new NotificationResponse(
            result.Value!.NotificationId,
            result.Value!.Title,
            result.Value!.Message,
            result.Value!.Data,
            result.Value!.WasRead,
            result.Value!.CreatedAt);

        return Ok(CollectionResponse<NotificationResponse>.From(response));
    }

    /// <summary>
    /// Marks notifications as read for the authenticated user.
    /// If notificationId is provided, marks only that notification as read.
    /// If notificationId is null, marks all notifications as read.
    /// </summary>
    [HttpPatch("read")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [Authorize(Policy = "notifications.update")]
    public async Task<IActionResult> MarkAsRead([FromQuery] Guid? notificationId)
    {
        _logger.LogInformation("Marking notifications as read. NotificationId: {NotificationId}", notificationId);

        var command = new UpdateRecipientWasReadCommand(notificationId);
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
        {
            return result.ToProblemDetails(this);
        }

        return NoContent();
    }

    /// <summary>
    /// Deletes a notification recipient (removes notification for the authenticated user)
    /// </summary>
    [HttpDelete("recipients/{notificationRecipientId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Policy = "notifications.delete")]
    public async Task<IActionResult> DeleteRecipient(Guid notificationRecipientId)
    {
        _logger.LogInformation("Deleting notification recipient: {NotificationRecipientId}", notificationRecipientId);

        var command = new DeleteRecipientCommand(notificationRecipientId);

        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
        {
            return result.ToProblemDetails(this);
        }

        return NoContent();
    }
}
