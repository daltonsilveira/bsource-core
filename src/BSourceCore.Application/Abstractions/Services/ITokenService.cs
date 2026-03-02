using BSourceCore.Application.Models.Requests;
using BSourceCore.Application.Models.Results;

namespace BSourceCore.Application.Abstractions.Services;

public interface ITokenService
{
    Task<TokenResult> GenerateTokenAsync(TokenSubject tokenData, CancellationToken cancellationToken = default);
    Task<TokenResult?> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
    Task RevokeTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
}
