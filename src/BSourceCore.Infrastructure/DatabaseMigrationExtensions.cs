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
        var logger = serviceProvider.GetRequiredService<ILoggerFactory>()
            .CreateLogger("DatabaseMigration");
        var applyMigrationsSetting = configuration.GetValue<bool?>("Database:ApplyMigrations");

        if (applyMigrationsSetting is null)
        {
            logger.LogWarning(
                "Database:ApplyMigrations not configured; defaulting to true. " +
                "Set Database:ApplyMigrations=false for multi-instance deployments.");
        }

        if (applyMigrationsSetting == false)
        {
            logger.LogInformation("Database migrations are disabled (Database:ApplyMigrations=false)");
            return;
        }

        using var scope = serviceProvider.CreateScope();
        try
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<WriteDbContext>();

            logger.LogInformation("Applying database migrations");
            dbContext.Database.Migrate();
            logger.LogInformation("Database migrations applied");
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Database migration failed. Verify the connection string, database permissions, " +
                "and pending migrations, or disable auto-migrations with Database:ApplyMigrations=false.");
            throw;
        }
    }
}
