using BSourceCore.Domain.Entities;
using BSourceCore.Domain.Enums;
using BSourceCore.Shared.Kernel.Security;
using Microsoft.EntityFrameworkCore;

namespace BSourceCore.Infrastructure.Persistence.Seed;

/// <summary>
/// Provides seed data for the Platform Core database
/// </summary>
public static class SeedData
{
    // Well-known IDs for seed data (deterministic for migrations)
    public static readonly Guid SystemTenantId = new("00000000-0000-0000-0000-000000000001");
    public static readonly Guid AdminUserId = new("00000000-0000-0000-0000-000000000002");
    public static readonly Guid AdminGroupId = new("00000000-0000-0000-0000-000000000003");

    /// <summary>
    /// Applies seed data to the model builder
    /// </summary>
    public static void ApplySeedData(ModelBuilder modelBuilder)
    {
        SeedTenant(modelBuilder);
        SeedPermissions(modelBuilder);
        SeedGroup(modelBuilder);
        SeedUser(modelBuilder);
        SeedUserGroup(modelBuilder);
    }

    private static void SeedTenant(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tenant>().HasData(new
        {
            TenantId = SystemTenantId,
            Name = "System",
            Slug = "system",
            Description = "System tenant for platform administration",
            Status = BaseStatus.Active,
            CreatedAt = DateTimeOffset.Parse("2024-01-01T00:00:00Z"),
            UpdatedAt = (DateTimeOffset?)null,
            CreatedBy = (Guid?)null,
            UpdatedBy = (Guid?)null
        });
    }

    private static void SeedPermissions(ModelBuilder modelBuilder)
    {
        var permissions = PermissionIdentifier.GetList().Select(o => new
        {
            PermissionId = o.Id,
            TenantId = SystemTenantId,
            Code = o.Code,
            Name = o.Name,
            Description = o.Description,
            Status = BaseStatus.Active,
            CreatedAt = DateTimeOffset.Parse("2024-01-01T00:00:00Z"),
            UpdatedAt = (DateTimeOffset?)null,
            CreatedBy = (Guid?)null,
            UpdatedBy = (Guid?)null
        });

        modelBuilder.Entity<Permission>().HasData(permissions);
    }

    private static void SeedGroup(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Group>().HasData(new
        {
            GroupId = AdminGroupId,
            TenantId = SystemTenantId,
            Name = "Administrators",
            Description = "Full system administrators with all permissions",
            IsSuperAdmin = true,
            Status = BaseStatus.Active,
            CreatedAt = DateTimeOffset.Parse("2024-01-01T00:00:00Z"),
            UpdatedAt = (DateTimeOffset?)null,
            CreatedBy = (Guid?)null,
            UpdatedBy = (Guid?)null
        });
    }

    private static void SeedUser(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData(new
        {
            UserId = AdminUserId,
            TenantId = SystemTenantId,
            Name = "System Administrator",
            Login = "admin",
            Email = "admin@bsource.local",
            PasswordHash = SeedPasswordHelper.DefaultAdminPasswordHash,
            IsFirstAccess = false,
            Status = BaseStatus.Active,
            CreatedAt = DateTimeOffset.Parse("2024-01-01T00:00:00Z"),
            UpdatedAt = (DateTimeOffset?)null,
            CreatedBy = (Guid?)null,
            UpdatedBy = (Guid?)null
        });
    }

    private static void SeedUserGroup(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserGroup>().HasData(new
        {
            UserGroupId = new Guid("00000000-0000-0000-0000-000000000100"),
            UserId = AdminUserId,
            GroupId = AdminGroupId,
            TenantId = SystemTenantId,
            CreatedAt = DateTimeOffset.Parse("2024-01-01T00:00:00Z"),
            UpdatedAt = (DateTimeOffset?)null,
            CreatedBy = (Guid?)null,
            UpdatedBy = (Guid?)null
        });
    }
}
