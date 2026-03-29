using BSourceCore.Application.Abstractions.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace BSourceCore.Infrastructure.Services;

/// <summary>
/// Handler de autorização que valida permissões do usuário
/// consultando o banco de dados ao invés de claims no JWT.
/// As permissões são cacheadas por usuário para evitar consultas repetidas ao banco.
/// </summary>
public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(5);

    private readonly IUserRepository _userRepository;
    private readonly IMemoryCache _cache;
    private readonly ILogger<PermissionAuthorizationHandler> _logger;

    public PermissionAuthorizationHandler(
        IUserRepository userRepository,
        IMemoryCache cache,
        ILogger<PermissionAuthorizationHandler> logger)
    {
        _userRepository = userRepository;
        _cache = cache;
        _logger = logger;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        var userIdClaim = context.User.FindFirst("userId")?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            _logger.LogWarning("Authorization failed: userId claim not found or invalid");
            return;
        }

        var permissionCodes = await GetUserPermissionCodesAsync(userId);

        if (permissionCodes.Contains(requirement.PermissionCode))
        {
            context.Succeed(requirement);
        }
        else
        {
            _logger.LogWarning(
                "Authorization failed: User {UserId} does not have permission '{Permission}'",
                userId,
                requirement.PermissionCode);
        }
    }

    private async Task<HashSet<string>> GetUserPermissionCodesAsync(Guid userId)
    {
        var cacheKey = $"permissions:{userId}";

        if (_cache.TryGetValue(cacheKey, out HashSet<string>? cached) && cached is not null)
        {
            return cached;
        }

        var permissions = await _userRepository.GetUserPermissionsAsync(userId);
        var codes = permissions.Select(p => p.Code).ToHashSet();

        _cache.Set(cacheKey, codes, CacheDuration);

        return codes;
    }
}
