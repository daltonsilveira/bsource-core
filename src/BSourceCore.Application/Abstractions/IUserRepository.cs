using BSourceCore.Domain.Entities;

namespace BSourceCore.Application.Abstractions;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByLoginAsync(string login, CancellationToken cancellationToken = default);
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<IEnumerable<User>> GetAllByTenantAsync(Guid tenantId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Permission>> GetUserPermissionsAsync(Guid userId, CancellationToken cancellationToken = default);    
}
