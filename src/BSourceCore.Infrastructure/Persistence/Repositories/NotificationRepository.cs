using BSourceCore.Application.Abstractions;
using BSourceCore.Application.Abstractions.Repositories;
using BSourceCore.Domain.Entities;
using BSourceCore.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BSourceCore.Infrastructure.Persistence.Repositories;

public class NotificationRepository : Repository<Notification>, INotificationRepository
{
    public NotificationRepository(
        WriteDbContext writeContext, 
        ReadOnlyDbContext readContext) : base(writeContext, readContext)
    {
    }

    public async Task<IEnumerable<Notification>> GetAllByUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _readContext.Notifications
            .Include(n => n.Recipients)
            .Where(n => n.Recipients.Any(r => r.UserId.Equals(userId)))
            .ToListAsync(cancellationToken);
    }

    public async Task<Notification?> GetByIdWithRecipientsAsync(Guid notificationId, CancellationToken cancellationToken = default)
    {
        return await _readContext.Notifications
            .Include(n => n.Recipients)
            .FirstOrDefaultAsync(n => n.NotificationId == notificationId, cancellationToken);
    }
}
