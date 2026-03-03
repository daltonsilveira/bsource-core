using Microsoft.AspNetCore.Authorization;

namespace BSourceCore.Infrastructure.Services;

/// <summary>
/// Requirement de autorização baseado em permissão.
/// Contém o código da permissão necessária para acessar o recurso.
/// </summary>
public class PermissionRequirement : IAuthorizationRequirement
{
    public string PermissionCode { get; }

    public PermissionRequirement(string permissionCode)
    {
        PermissionCode = permissionCode;
    }
}
