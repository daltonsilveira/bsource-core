using System.Threading;

namespace BSourceCore.Shared.Abstractions;

public interface IDatabaseMigrationService
{
    Task ApplyMigrationsAsync(CancellationToken cancellationToken = default);
}
