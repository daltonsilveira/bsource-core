using BSourceCore.Application.Abstractions.Repositories;
using BSourceCore.Application.Features.Notifications.DTOs;
using BSourceCore.Shared.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BSourceCore.Application.Features.Users.Queries.GetCurrentUser;

public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, CurrentUserDto?>
{
    private readonly IUserRepository _userRepository;
    private readonly INotificationRepository _notificationRepository;
    private readonly IUserContext _userContext;
    private readonly ILogger<GetCurrentUserQueryHandler> _logger;

    public GetCurrentUserQueryHandler(
        IUserRepository userRepository,
        INotificationRepository notificationRepository,
        IUserContext userContext,
        ILogger<GetCurrentUserQueryHandler> logger)
    {
        _userRepository = userRepository;
        _notificationRepository = notificationRepository;
        _userContext = userContext;
        _logger = logger;
    }

    public async Task<CurrentUserDto?> Handle(
        GetCurrentUserQuery request,
        CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId;

        if (userId is null)
        {
            _logger.LogWarning("Attempted to get current user without authenticated context");
            return null;
        }

        _logger.LogInformation("Getting current user data for: {UserId}", userId);

        var user = await _userRepository.GetByIdAsync(userId.Value, cancellationToken);

        if (user is null)
        {
            _logger.LogWarning("Authenticated user not found in database: {UserId}", userId);
            return null;
        }

        var permissions = await _userRepository.GetUserPermissionsAsync(userId.Value, cancellationToken);

        var notifications = await _notificationRepository.ListByUserAsync(userId.Value, cancellationToken);

        var notificationDtos = notifications.Select(n => new NotificationDto(
            n.NotificationId,
            n.Title,
            n.Message,
            n.Data,
            n.Recipients.Any(r => r.UserId == userId && r.WasRead),
            n.CreatedAt,
            n.Recipients.Where(r => r.UserId == userId).Select(r => r.NotificationRecipientId).FirstOrDefault()));

        return new CurrentUserDto(
            user.UserId,
            user.TenantId,
            user.Name,
            user.Email,
            permissions.Select(p => p.Code),
            notificationDtos);
    }
}
