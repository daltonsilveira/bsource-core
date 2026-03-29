using BSourceCore.Infrastructure.Authorization;
using BSourceCore.Infrastructure.Services;
using BSourceCore.Shared.Kernel.Security;
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
        services.AddAuthorization(options =>
        {
            foreach (var permission in PermissionIdentifier.GetList())
            {
                options.AddPolicy(permission.Code, policy =>
                    policy.Requirements.Add(new PermissionRequirement(permission.Code)));
            }
        });

        return services;
    }
}