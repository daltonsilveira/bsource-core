using BSourceCore.Domain.Entities;

namespace BSourceCore.Application.Abstractions.Repositories;

public interface INotificationRecipientRepository : IRepository<NotificationRecipient>
{
    Task<IEnumerable<NotificationRecipient>> ListByUserAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<NotificationRecipient>> ListByNotificationAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<NotificationRecipient?> GetByNotificationAndUserAsync(Guid notificationId, Guid userId, CancellationToken cancellationToken = default);
}
