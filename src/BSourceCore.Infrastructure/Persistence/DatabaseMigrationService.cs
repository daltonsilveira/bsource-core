using System.Threading;
using BSourceCore.Shared.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BSourceCore.Infrastructure.Persistence;

public class DatabaseMigrationService : IDatabaseMigrationService
{
    private readonly WriteDbContext _dbContext;
    private readonly IConfiguration _configuration;
    private readonly ILogger<DatabaseMigrationService> _logger;

    public DatabaseMigrationService(
        WriteDbContext dbContext,
        IConfiguration configuration,
        ILogger<DatabaseMigrationService> logger)
    {
        _dbContext = dbContext;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task ApplyMigrationsAsync(CancellationToken cancellationToken = default)
    {
        var applyMigrationsSetting = _configuration.GetValue<bool?>("Database:ApplyMigrations");

        if (applyMigrationsSetting is null)
        {
            _logger.LogWarning(
                "Database:ApplyMigrations not configured; defaulting to false. " +
                "Set Database:ApplyMigrations=true to enable automatic migrations.");
        }

        if (applyMigrationsSetting != true)
        {
            _logger.LogInformation("Database migrations are disabled (Database:ApplyMigrations=false)");
            return;
        }

        try
        {
            _logger.LogInformation("Applying database migrations");
            await _dbContext.Database.MigrateAsync(cancellationToken);
            _logger.LogInformation("Database migrations applied");
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Database migration failed. Verify the connection string, database permissions, " +
                "and pending migrations, or disable auto-migrations with Database:ApplyMigrations=false.");
            throw;
        }
    }
}
