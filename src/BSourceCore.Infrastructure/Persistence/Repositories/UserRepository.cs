using BSourceCore.Application.Abstractions;
using BSourceCore.Application.Abstractions.Repositories;
using BSourceCore.Domain.Entities;
using BSourceCore.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BSourceCore.Infrastructure.Persistence.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(
        WriteDbContext writeContext, 
        ReadOnlyDbContext readContext) : base(writeContext, readContext)
    {
    }

    public async Task<User?> GetByLoginAsync(string login, CancellationToken cancellationToken = default)
    {
        return await _readContext.Users
            .FirstOrDefaultAsync(u => u.Login == login, cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _readContext.Users
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<IEnumerable<User>> GetAllByTenantAsync(Guid tenantId, CancellationToken cancellationToken = default)
    {
        return await _readContext.Users
            .Where(u => u.TenantId == tenantId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Permission>> GetUserPermissionsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        // Get all permissions through user's groups
        var permissions = await _readContext.UserGroups
            .Where(ug => ug.UserId == userId)
            .SelectMany(ug => ug.Group.GroupPermissions)
            .Select(gp => gp.Permission)
            .Distinct()
            .ToListAsync(cancellationToken);

        return permissions;
    }
}
