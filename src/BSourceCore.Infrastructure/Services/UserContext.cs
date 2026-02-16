using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BSourceCore.Shared.Abstractions;
using Microsoft.AspNetCore.Http;

namespace BSourceCore.Infrastructure.Services;

/// <summary>
/// Implementação do contexto de usuário
/// Extrai informações do HttpContext
/// </summary>
public class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid? UserId
    {
        get
        {
            var claim = _httpContextAccessor.HttpContext?.User?.FindFirst("userId")?.Value;
            return Guid.TryParse(claim, out var userId) ? userId : null;
        }
    }

    public Guid? TenantId
    {
        get
        {
            var claim = _httpContextAccessor.HttpContext?.User?.FindFirst("tenantId")?.Value;
            return Guid.TryParse(claim, out var tenantId) ? tenantId : null;
        }
    }

    public string? Email => _httpContextAccessor.HttpContext?.User?.FindFirst(JwtRegisteredClaimNames.Email)?.Value;

    public string? IpAddress => _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();

    public string? UserAgent => _httpContextAccessor.HttpContext?.Request?.Headers["User-Agent"].ToString();

    public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
}
