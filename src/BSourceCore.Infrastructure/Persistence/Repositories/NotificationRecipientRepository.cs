using BSourceCore.Application.Abstractions;
using BSourceCore.Application.Abstractions.Repositories;
using BSourceCore.Domain.Entities;
using BSourceCore.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BSourceCore.Infrastructure.Persistence.Repositories;

public class NotificationRecipientRepository : Repository<NotificationRecipient>, INotificationRecipientRepository
{
    public NotificationRecipientRepository(
        WriteDbContext writeContext, 
        ReadOnlyDbContext readContext) : base(writeContext, readContext)
    {
    }

    public async Task<IEnumerable<NotificationRecipient>> ListByUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _readContext.NotificationRecipients
            .Where(nr => nr.UserId.Equals(userId))
            .ToListAsync(cancellationToken);
    }

    public async Task<NotificationRecipient?> GetByNotificationAsync(Guid notificationId, Guid userId, CancellationToken cancellationToken = default)
    {
        return await _readContext.NotificationRecipients
            .Where(nr => nr.NotificationId.Equals(notificationId)
                      && nr.UserId.Equals(userId))
            .FirstOrDefaultAsync(cancellationToken);
    }
}
