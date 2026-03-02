using BSourceCore.Domain.Entities;

namespace BSourceCore.Application.Abstractions.Repositories;

public interface IUserGroupRepository  : IRepository<UserGroup>
{
    Task<UserGroup?> GetAsync(Guid userId, Guid groupId, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserGroup>> ListByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserGroup>> ListByGroupIdAsync(Guid groupId, CancellationToken cancellationToken = default);    
    Task<IEnumerable<UserGroup>> ListByGroupIdsAsync(List<Guid> groupIds, CancellationToken cancellationToken = default);
}
