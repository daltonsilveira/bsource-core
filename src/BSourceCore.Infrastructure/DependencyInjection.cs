using BSourceCore.Application.Abstractions;
using BSourceCore.Application.Abstractions.Repositories;
using BSourceCore.Application.Abstractions.Services;
using BSourceCore.Infrastructure.Options;
using BSourceCore.Infrastructure.Persistence;
using BSourceCore.Infrastructure.Persistence.Repositories;
using BSourceCore.Infrastructure.Repositories;
using BSourceCore.Infrastructure.Services;
using BSourceCore.Shared.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BSourceCore.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        services.AddDbContext<WriteDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddDbContext<ReadOnlyDbContext>(options =>
            options.UseNpgsql(connectionString));

        // HttpContextAccessor for UserContext
        services.AddHttpContextAccessor();

        // Options
        services.Configure<BSourceNotifierOptions>(
            configuration.GetSection(BSourceNotifierOptions.SectionName));

        // Repositories
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ITenantRepository, TenantRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IGroupRepository, GroupRepository>();
        services.AddScoped<IPermissionRepository, PermissionRepository>();
        services.AddScoped<IUserGroupRepository, UserGroupRepository>();
        services.AddScoped<IGroupPermissionRepository, GroupPermissionRepository>();
        services.AddScoped<IPasswordResetRepository, PasswordResetRepository>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<INotificationRecipientRepository, NotificationRecipientRepository>();

        // Services
        services.AddScoped<ITenantContext, TenantContext>();
        services.AddScoped<IUserContext, UserContext>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IWebSocketNotificationService, WebSocketNotificationService>();

        // HttpClients        
        services.AddHttpClient("BSourceNotifier", (serviceProvider, client) =>
        {
            var settings = serviceProvider
                .GetRequiredService<IOptions<BSourceNotifierOptions>>().Value;

            client.BaseAddress = new Uri(settings.BaseUrl);
        });

        return services;
    }
}
