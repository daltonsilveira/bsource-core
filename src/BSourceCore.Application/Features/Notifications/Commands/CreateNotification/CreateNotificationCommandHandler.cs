using System.Dynamic;
using BSourceCore.Application.Abstractions;
using BSourceCore.Application.Abstractions.Repositories;
using BSourceCore.Application.Abstractions.Services;
using BSourceCore.Domain.Entities;
using BSourceCore.Shared.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BSourceCore.Application.Features.Notifications.Commands.CreateNotification;

public class CreateNotificationCommandHandler : IRequestHandler<CreateNotificationCommand, CreateNotificationResult>
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IUserGroupRepository _userGroupRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;
    private readonly IEmailService _emailService;
    private readonly IWebSocketNotificationService _webSocketNotificationService;
    private readonly ILogger<CreateNotificationCommandHandler> _logger;

    public CreateNotificationCommandHandler(
        INotificationRepository notificationRepository,
        IUserGroupRepository userGroupRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext,
        IEmailService emailService,
        IWebSocketNotificationService webSocketNotificationService,
        ILogger<CreateNotificationCommandHandler> logger)
    {
        _notificationRepository = notificationRepository;
        _userGroupRepository = userGroupRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _userContext = userContext;
        _emailService = emailService;
        _webSocketNotificationService = webSocketNotificationService;
        _logger = logger;
    }

    public async Task<CreateNotificationResult> Handle(
        CreateNotificationCommand request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating notification with title: {Title}", request.Title);

        var notification = new Notification(
            request.Title,
            request.Message,
            request.Data,
            _userContext.UserId);

        // Coletar todos os UserIds destinatários (diretos + expandidos por RoleGroup)
        var allUserIds = new HashSet<Guid>(request.UserIds ?? []);

        if (request.GroupIds is not null && request.GroupIds.Count > 0)
        {
            var usersByGroup = await _userGroupRepository.ListByGroupIdsAsync(request.GroupIds, cancellationToken);

            foreach (var userGroup in usersByGroup)
            {
                allUserIds.Add(userGroup.UserId);
            }
        }

        // Criar NotificationRecipient para cada usuário único
        foreach (var userId in allUserIds)
        {
            notification.Recipients.Add(new NotificationRecipient(
                notification.NotificationId,
                userId,
                _userContext.UserId));
        }


        await _notificationRepository.AddAsync(notification, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Notification created with Id: {NotificationId} and sent to {RecipientCount} recipients",
            notification.NotificationId,
            notification.Recipients.Count);
            
        await SendToNotifier(notification, request.Types.ToList(), cancellationToken);

        return new CreateNotificationResult();
    }

    public static dynamic MergeObjects(dynamic item1, dynamic item2)
    {
        var dictionary1 = (IDictionary<string, object>)item1;
        var dictionary2 = (IDictionary<string, object>)item2;
        var result = new ExpandoObject() as IDictionary<string, object>;

        // Add all properties from both objects. Handles collisions by taking the last value added.
        foreach (var pair in dictionary1.Concat(dictionary2))
        {
            result[pair.Key] = pair.Value;
        }

        return result;
    }

    /// <summary>
    /// Envia a notificação para o serviço de notificações, que irá encaminhar para os destinatários conforme os tipos especificados (WebSocket, Email, etc).
    /// </summary>
    private async Task SendToNotifier(
        Notification notification,
        List<CreateNotificationType> recipientTypes,
        CancellationToken cancellationToken)
    {
        foreach (var recipient in notification.Recipients)
        {
            try
            {
                object notificationData = null;
                if (!string.IsNullOrEmpty(notification.Data))
                {
                    try
                    {
                        notificationData = System.Text.Json.JsonSerializer.Deserialize<object>(notification.Data);
                    }
                    catch
                    {
                        notificationData = new { raw = notification.Data };
                    }
                }

                var notificationIds = new
                {
                    notification.NotificationId,
                    recipient.NotificationRecipientId,
                    recipient.WasRead,
                    notification.CreatedAt
                };

                notificationData = MergeObjects(notificationData ?? new { }, notificationIds);

                if (recipientTypes.Contains(CreateNotificationType.WebSocket))
                {
                    await _webSocketNotificationService.SendNotification(
                        notification.Title,
                        notification.Message,
                        recipient.UserId,
                        notificationData,
                        cancellationToken);
                }

                if (recipientTypes.Contains(CreateNotificationType.Email))
                {
                    var user = await _userRepository.GetByIdAsync(recipient.UserId, cancellationToken);

                    if(user is null)
                    {
                        _logger.LogWarning("User with Id {UserId} not found for email notification", recipient.UserId);
                        continue;
                    }

                    await _emailService.SendEmail(
                        user.Email,
                        notification.Title,
                        notification.Message,
                        notificationData,
                        cancellationToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao enviar notificação WebSocket para o usuário {UserId}", recipient.UserId);
            }
        }
    }
}
