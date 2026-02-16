using BSourceCore.Domain.Entities;

namespace BSourceCore.Application.Abstractions;

public interface IUserGroupRepository  : IRepository<UserGroup>
{
    Task<UserGroup?> GetAsync(Guid userId, Guid groupId, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserGroup>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserGroup>> GetByGroupIdAsync(Guid groupId, CancellationToken cancellationToken = default);    
}
