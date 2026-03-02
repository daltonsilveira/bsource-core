using BSourceCore.Domain.Entities;

namespace BSourceCore.Application.Abstractions.Repositories;

public interface INotificationRepository : IRepository<Notification>
{
    Task<IEnumerable<Notification>> GetAllByUserAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<Notification?> GetByIdWithRecipientsAsync(Guid notificationId, CancellationToken cancellationToken = default);
}
