using BSourceCore.Domain.Entities;

namespace BSourceCore.Application.Abstractions.Repositories;

public interface IPasswordResetRepository : IRepository<PasswordReset>
{
    Task<PasswordReset?> GetByTokenAsync(string token, CancellationToken cancellationToken = default);
    Task<IEnumerable<PasswordReset>> GetActiveByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
}
