namespace BSourceCore.Shared.Abstractions;

public interface IDatabaseMigrationService
{
    void ApplyMigrations();
}
