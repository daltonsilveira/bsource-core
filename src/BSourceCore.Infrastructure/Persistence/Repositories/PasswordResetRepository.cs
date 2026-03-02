using BSourceCore.Application.Abstractions;
using BSourceCore.Application.Abstractions.Repositories;
using BSourceCore.Domain.Entities;
using BSourceCore.Domain.Enums;
using BSourceCore.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BSourceCore.Infrastructure.Persistence.Repositories;

public class PasswordResetRepository : Repository<PasswordReset>, IPasswordResetRepository
{
    public PasswordResetRepository(
        WriteDbContext writeContext,
        ReadOnlyDbContext readContext) : base(writeContext, readContext)
    {
    }

    public async Task<PasswordReset?> GetByTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        return await _readContext.PasswordResets
            .Include(pr => pr.User)
            .FirstOrDefaultAsync(pr => pr.Token == token, cancellationToken);
    }

    public async Task<IEnumerable<PasswordReset>> GetActiveByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _readContext.PasswordResets
            .Where(pr => pr.UserId == userId && pr.Status == BaseStatus.Active)
            .ToListAsync(cancellationToken);
    }
}
