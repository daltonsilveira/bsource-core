using BSourceCore.Application.Models.Results;
using BSourceCore.Domain.Entities;

namespace BSourceCore.Application.Abstractions.Services;

public interface ITokenService
{
    Task<TokenResult> GenerateTokenAsync(User user, IEnumerable<Permission> permissions, CancellationToken cancellationToken = default);
    Task<TokenResult?> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
    Task RevokeTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
}
