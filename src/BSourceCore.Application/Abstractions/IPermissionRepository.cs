using BSourceCore.Domain.Entities;

namespace BSourceCore.Application.Abstractions;

public interface IPermissionRepository : IRepository<Permission>
{
    Task<Permission?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
    Task<IEnumerable<Permission>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default);
}
