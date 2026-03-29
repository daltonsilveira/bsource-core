using Microsoft.AspNetCore.Authorization;

namespace BSourceCore.API.Attributes;

/// <summary>
/// Atributo de autorização baseado em permissão.
/// Substitui [Authorize(Policy = "permission.code")] por [HasPermission("permission.code")],
/// tornando explícito que o acesso é controlado por permissão do sistema.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class HasPermissionAttribute : AuthorizeAttribute
{
    public HasPermissionAttribute(string permission)
    {
        Policy = permission;
    }
}
