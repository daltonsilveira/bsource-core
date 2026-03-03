using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BSourceCore.Application.Abstractions;
using BSourceCore.Application.Abstractions.Services;
using BSourceCore.Application.Models.Requests;
using BSourceCore.Application.Models.Results;
using BSourceCore.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BSourceCore.Infrastructure.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Task<TokenResult> GenerateTokenAsync(TokenSubject tokenData, CancellationToken cancellationToken = default)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"] ?? "BSourceCore-Super-Secret-Key-For-Development-Only-Must-Be-At-Least-32-Characters";
        var issuer = jwtSettings["Issuer"] ?? "BSourceCore";
        var audience = jwtSettings["Audience"] ?? "BSourceCore";
        var expirationMinutes = int.Parse(jwtSettings["ExpirationMinutes"] ?? "60");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, tokenData.UserId.ToString()),
            new(JwtRegisteredClaimNames.Email, tokenData.Email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new("userId", tokenData.UserId.ToString()),
            new("name", tokenData.Name),
            new("tenantId", tokenData.TenantId.ToString())
        };

        var expiresAt = DateTime.UtcNow.AddMinutes(expirationMinutes);
        
        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: credentials);

        var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
        var refreshToken = GenerateRefreshToken();

        return Task.FromResult(new TokenResult(
            accessToken,
            refreshToken,
            new DateTimeOffset(expiresAt, TimeSpan.Zero)));
    }

    public Task<TokenResult?> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        // In a real implementation, you would:
        // 1. Validate the refresh token against a store
        // 2. Get the user associated with the refresh token
        // 3. Generate new tokens
        // For now, we'll return null to indicate invalid token
        return Task.FromResult<TokenResult?>(null);
    }

    public Task RevokeTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        // In a real implementation, you would remove the refresh token from the store
        return Task.CompletedTask;
    }

    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}
