using System.Security.Cryptography;

namespace BSourceCore.Infrastructure.Persistence.Seed;

/// <summary>
/// Helper to generate password hashes for seed data
/// </summary>
public static class SeedPasswordHelper
{
    private const int SaltSize = 16;
    private const int HashSize = 32;
    private const int Iterations = 100000;
    private static readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA256;

    /// <summary>
    /// Generates a deterministic password hash for seed data.
    /// Uses a fixed salt for seed data to ensure consistent migrations.
    /// </summary>
    public static string GenerateSeedPasswordHash(string password)
    {
        // Fixed salt for seed data (deterministic for migrations)
        // This ensures the same password always generates the same hash in seed data
        var salt = new byte[] { 0x78, 0x58, 0x4D, 0x4F, 0x54, 0x58, 0x45, 0x56, 0x76, 0x35, 0x4F, 0x71, 0x51, 0x7A, 0x4D, 0x6D };
        var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algorithm, HashSize);

        return $"{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
    }

    /// <summary>
    /// The default admin password for seed data.
    /// IMPORTANT: Change this password immediately after first deployment!
    /// </summary>
    public const string DefaultAdminPassword = "123456";

    /// <summary>
    /// Pre-computed hash for the default admin password.
    /// This is computed with a deterministic salt for migration stability.
    /// </summary>
    public static string DefaultAdminPasswordHash => GenerateSeedPasswordHash(DefaultAdminPassword);
}
