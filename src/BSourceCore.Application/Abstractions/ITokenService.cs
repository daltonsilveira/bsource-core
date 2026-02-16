using BSourceCore.Domain.Entities;

namespace BSourceCore.Application.Abstractions;

public interface ITokenService
{
    Task<TokenResult> GenerateTokenAsync(User user, IEnumerable<Permission> permissions, CancellationToken cancellationToken = default);
    Task<TokenResult?> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
    Task RevokeTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
}

public record TokenResult(
    string AccessToken,
    string RefreshToken,
    DateTimeOffset ExpiresAt);
