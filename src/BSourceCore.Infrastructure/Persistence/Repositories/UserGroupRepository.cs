using BSourceCore.Application.Abstractions;
using BSourceCore.Domain.Entities;
using BSourceCore.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BSourceCore.Infrastructure.Repositories;

public class UserGroupRepository : Repository<UserGroup>, IUserGroupRepository
{
    public UserGroupRepository(
        WriteDbContext writeContext, 
        ReadOnlyDbContext readContext) : base(writeContext, readContext)
    {
    }

    public async Task<UserGroup?> GetAsync(Guid userId, Guid groupId, CancellationToken cancellationToken = default)
    {
        return await _readContext.UserGroups
            .FirstOrDefaultAsync(ug => ug.UserId == userId && ug.GroupId == groupId, cancellationToken);
    }

    public async Task<IEnumerable<UserGroup>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _readContext.UserGroups
            .Where(ug => ug.UserId == userId)
            .Include(ug => ug.Group)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<UserGroup>> GetByGroupIdAsync(Guid groupId, CancellationToken cancellationToken = default)
    {
        return await _readContext.UserGroups
            .Where(ug => ug.GroupId == groupId)
            .Include(ug => ug.User)
            .ToListAsync(cancellationToken);
    }
}
