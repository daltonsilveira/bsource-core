using BSourceCore.Infrastructure.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace BSourceCore.API.Extensions;

public static class AuthorizationExtensions
{
    /// <summary>
    /// Configura autorização baseada em permissões com provider dinâmico de políticas.
    /// Políticas são criadas automaticamente a partir do código da permissão,
    /// eliminando a necessidade de pré-registrar cada permissão individualmente.
    /// </summary>
    public static IServiceCollection AddPolicyAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization();
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

        return services;
    }
}