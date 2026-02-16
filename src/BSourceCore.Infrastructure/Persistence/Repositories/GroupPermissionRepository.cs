using BSourceCore.Application.Abstractions;
using BSourceCore.Domain.Entities;
using BSourceCore.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BSourceCore.Infrastructure.Repositories;

public class GroupPermissionRepository : Repository<GroupPermission>, IGroupPermissionRepository
{
    public GroupPermissionRepository(
        WriteDbContext writeContext, 
        ReadOnlyDbContext readContext) : base(writeContext, readContext)
    {
    }

    public async Task<GroupPermission?> GetAsync(Guid groupId, Guid permissionId, CancellationToken cancellationToken = default)
    {
        return await _readContext.GroupPermissions
            .FirstOrDefaultAsync(gp => gp.GroupId == groupId && gp.PermissionId == permissionId, cancellationToken);
    }

    public async Task<IEnumerable<GroupPermission>> GetByGroupIdAsync(Guid groupId, CancellationToken cancellationToken = default)
    {
        return await _readContext.GroupPermissions
            .Where(gp => gp.GroupId == groupId)
            .Include(gp => gp.Permission)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<GroupPermission>> GetByPermissionIdAsync(Guid permissionId, CancellationToken cancellationToken = default)
    {
        return await _readContext.GroupPermissions
            .Where(gp => gp.PermissionId == permissionId)
            .Include(gp => gp.Group)
            .ToListAsync(cancellationToken);
    }
}
