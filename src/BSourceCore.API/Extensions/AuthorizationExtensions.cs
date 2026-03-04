using BSourceCore.Infrastructure.Services;

namespace BSourceCore.API.Extensions;

public static class AuthorizationExtensions
{
    public static IServiceCollection AddPolicyAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            // Tenant policies
            options.AddPolicy("tenants.create", policy => policy.Requirements.Add(new PermissionRequirement("tenants.create")));
            options.AddPolicy("tenants.read", policy => policy.Requirements.Add(new PermissionRequirement("tenants.read")));
            options.AddPolicy("tenants.update", policy => policy.Requirements.Add(new PermissionRequirement("tenants.update")));
            options.AddPolicy("tenants.delete", policy => policy.Requirements.Add(new PermissionRequirement("tenants.delete")));

            // User policies
            options.AddPolicy("users.create", policy => policy.Requirements.Add(new PermissionRequirement("users.create")));
            options.AddPolicy("users.read", policy => policy.Requirements.Add(new PermissionRequirement("users.read")));
            options.AddPolicy("users.update", policy => policy.Requirements.Add(new PermissionRequirement("users.update")));
            options.AddPolicy("users.delete", policy => policy.Requirements.Add(new PermissionRequirement("users.delete")));

            // Group policies
            options.AddPolicy("groups.create", policy => policy.Requirements.Add(new PermissionRequirement("groups.create")));
            options.AddPolicy("groups.read", policy => policy.Requirements.Add(new PermissionRequirement("groups.read")));
            options.AddPolicy("groups.update", policy => policy.Requirements.Add(new PermissionRequirement("groups.update")));
            options.AddPolicy("groups.delete", policy => policy.Requirements.Add(new PermissionRequirement("groups.delete")));

            // Permission policies
            options.AddPolicy("permissions.create", policy => policy.Requirements.Add(new PermissionRequirement("permissions.create")));
            options.AddPolicy("permissions.read", policy => policy.Requirements.Add(new PermissionRequirement("permissions.read")));
            options.AddPolicy("permissions.update", policy => policy.Requirements.Add(new PermissionRequirement("permissions.update")));
            options.AddPolicy("permissions.delete", policy => policy.Requirements.Add(new PermissionRequirement("permissions.delete")));
        });

        return services;
    }
}