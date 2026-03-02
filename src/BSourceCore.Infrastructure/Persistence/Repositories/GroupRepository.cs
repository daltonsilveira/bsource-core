using BSourceCore.Application.Abstractions;
using BSourceCore.Application.Abstractions.Repositories;
using BSourceCore.Domain.Entities;
using BSourceCore.Domain.Enums;
using BSourceCore.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BSourceCore.Infrastructure.Repositories;

public class GroupRepository : Repository<Group>, IGroupRepository
{
    public GroupRepository(
        WriteDbContext writeContext, 
        ReadOnlyDbContext readContext) : base(writeContext, readContext)
    {
    }

    public async Task<Group?> GetByNameAsync(Guid tenantId, string name, CancellationToken cancellationToken = default)
    {
        return await _readContext.Groups
            .FirstOrDefaultAsync(g => g.TenantId == tenantId && g.Name == name && g.Status != BaseStatus.Deleted, cancellationToken);
    }

    public async Task<IEnumerable<Group>> GetAllByTenantAsync(Guid tenantId, CancellationToken cancellationToken = default)
    {
        return await _readContext.Groups
            .Where(g => g.TenantId == tenantId && g.Status != BaseStatus.Deleted)
            .ToListAsync(cancellationToken);
    }
}
