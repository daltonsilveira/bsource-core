using BSourceCore.Application.Abstractions.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace BSourceCore.Infrastructure.Services;

/// <summary>
/// Handler de autorização que valida permissões do usuário
/// consultando o banco de dados ao invés de claims no JWT.
/// </summary>
public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<PermissionAuthorizationHandler> _logger;

    public PermissionAuthorizationHandler(
        IUserRepository userRepository,
        ILogger<PermissionAuthorizationHandler> logger)
    {
        _userRepository = userRepository;
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

        var permissions = await _userRepository.GetUserPermissionsAsync(userId);

        if (permissions.Any(p => p.Code == requirement.PermissionCode))
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
}
