using BSourceCore.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BSourceCore.Infrastructure;

public static class DatabaseMigrationExtensions
{
    public static void ApplyDatabaseMigrations(this IServiceProvider serviceProvider, IConfiguration configuration)
    {
        if (!configuration.GetValue("Database:ApplyMigrations", true))
        {
            var skipLogger = serviceProvider.GetRequiredService<ILoggerFactory>()
                .CreateLogger("DatabaseMigration");
            skipLogger.LogInformation("Database migrations are disabled (Database:ApplyMigrations=false)");
            return;
        }

        using var scope = serviceProvider.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<WriteDbContext>>();
        try
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<WriteDbContext>();

            logger.LogInformation("Applying database migrations");
            dbContext.Database.Migrate();
            logger.LogInformation("Database migrations applied");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Database migration failed");
            throw;
        }
    }
}
