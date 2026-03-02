namespace BSourceCore.Application.Abstractions.Services;

public interface IWebSocketNotificationService
{
    /// <summary>
    /// Envia uma notificação via WebSocket para um usuário específico  
    /// </summary>
    /// <param name="title"></param>
    /// <param name="message"></param>
    /// <param name="userId"></param>
    /// <param name="data"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task SendNotification(string title, string message, Guid userId, object? data = null, CancellationToken cancellationToken = default);
}
