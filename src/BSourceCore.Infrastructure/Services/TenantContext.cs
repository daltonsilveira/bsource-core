using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BSourceCore.Shared.Abstractions;
using Microsoft.AspNetCore.Http;

namespace BSourceCore.Infrastructure.Services;

/// <summary>
/// Implementação do contexto de tenant
/// Extrai informações do HttpContext
/// </summary>
public class TenantContext : ITenantContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TenantContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid? TenantId
    {
        get
        {
            var claim = _httpContextAccessor.HttpContext?.User?.FindFirst("tenantId")?.Value;
            return Guid.TryParse(claim, out var tenantId) ? tenantId : null;
        }
    }
}
