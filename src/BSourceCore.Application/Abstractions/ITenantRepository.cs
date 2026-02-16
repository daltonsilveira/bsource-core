using BSourceCore.Domain.Entities;

namespace BSourceCore.Application.Abstractions;

public interface ITenantRepository : IRepository<Tenant>
{
    Task<Tenant?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
}
