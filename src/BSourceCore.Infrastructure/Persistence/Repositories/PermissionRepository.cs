using BSourceCore.Application.Abstractions;
using BSourceCore.Domain.Entities;
using BSourceCore.Domain.Enums;
using BSourceCore.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BSourceCore.Infrastructure.Repositories;

public class PermissionRepository : Repository<Permission>, IPermissionRepository
{
    public PermissionRepository(
        WriteDbContext writeContext, 
        ReadOnlyDbContext readContext) : base(writeContext, readContext)
    {
    }

    public async Task<Permission?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        return await _readContext.Permissions
            .FirstOrDefaultAsync(p => p.Code == code && p.Status != BaseStatus.Deleted, cancellationToken);
    }

    public async Task<IEnumerable<Permission>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
    {
        return await _readContext.Permissions
            .Where(p => ids.Contains(p.PermissionId) && p.Status != BaseStatus.Deleted)
            .ToListAsync(cancellationToken);
    }
}
