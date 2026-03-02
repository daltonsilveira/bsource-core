using BSourceCore.Domain.Entities;

namespace BSourceCore.Application.Abstractions.Repositories;

public interface IGroupRepository : IRepository<Group>
{
    Task<Group?> GetByNameAsync(Guid tenantId, string name, CancellationToken cancellationToken = default);
    Task<IEnumerable<Group>> GetAllByTenantAsync(Guid tenantId, CancellationToken cancellationToken = default);
}
