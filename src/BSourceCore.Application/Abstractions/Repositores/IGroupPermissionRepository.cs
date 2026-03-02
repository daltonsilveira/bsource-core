using BSourceCore.Domain.Entities;

namespace BSourceCore.Application.Abstractions.Repositories;

public interface IGroupPermissionRepository : IRepository<GroupPermission>
{
    Task<GroupPermission?> GetAsync(Guid groupId, Guid permissionId, CancellationToken cancellationToken = default);
    Task<IEnumerable<GroupPermission>> GetByGroupIdAsync(Guid groupId, CancellationToken cancellationToken = default);
    Task<IEnumerable<GroupPermission>> GetByPermissionIdAsync(Guid permissionId, CancellationToken cancellationToken = default);
}
