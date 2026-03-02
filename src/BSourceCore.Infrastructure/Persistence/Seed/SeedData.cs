using BSourceCore.Domain.Entities;
using BSourceCore.Domain.Enums;
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

    // Permission IDs
    public static readonly Guid TenantCreatePermissionId = new("10000000-0000-0000-0000-000000000001");
    public static readonly Guid TenantReadPermissionId = new("10000000-0000-0000-0000-000000000002");
    public static readonly Guid TenantUpdatePermissionId = new("10000000-0000-0000-0000-000000000003");
    public static readonly Guid TenantDeletePermissionId = new("10000000-0000-0000-0000-000000000004");

    public static readonly Guid UserCreatePermissionId = new("20000000-0000-0000-0000-000000000001");
    public static readonly Guid UserReadPermissionId = new("20000000-0000-0000-0000-000000000002");
    public static readonly Guid UserUpdatePermissionId = new("20000000-0000-0000-0000-000000000003");
    public static readonly Guid UserDeletePermissionId = new("20000000-0000-0000-0000-000000000004");

    public static readonly Guid GroupCreatePermissionId = new("30000000-0000-0000-0000-000000000001");
    public static readonly Guid GroupReadPermissionId = new("30000000-0000-0000-0000-000000000002");
    public static readonly Guid GroupUpdatePermissionId = new("30000000-0000-0000-0000-000000000003");
    public static readonly Guid GroupDeletePermissionId = new("30000000-0000-0000-0000-000000000004");

    public static readonly Guid PermissionCreatePermissionId = new("40000000-0000-0000-0000-000000000001");
    public static readonly Guid PermissionReadPermissionId = new("40000000-0000-0000-0000-000000000002");
    public static readonly Guid PermissionUpdatePermissionId = new("40000000-0000-0000-0000-000000000003");
    public static readonly Guid PermissionDeletePermissionId = new("40000000-0000-0000-0000-000000000004");

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
        SeedGroupPermissions(modelBuilder);
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
        var permissions = new[]
        {
            // Tenant permissions
            new { PermissionId = TenantCreatePermissionId, TenantId = SystemTenantId, Code = "tenants.create", Name = "Create Tenant", Description = "Permission to create tenants", Status = BaseStatus.Active, CreatedAt = DateTimeOffset.Parse("2024-01-01T00:00:00Z"), UpdatedAt = (DateTimeOffset?)null, CreatedBy = (Guid?)null, UpdatedBy = (Guid?)null },
            new { PermissionId = TenantReadPermissionId, TenantId = SystemTenantId, Code = "tenants.read", Name = "Read Tenant", Description = "Permission to read tenants", Status = BaseStatus.Active, CreatedAt = DateTimeOffset.Parse("2024-01-01T00:00:00Z"), UpdatedAt = (DateTimeOffset?)null, CreatedBy = (Guid?)null, UpdatedBy = (Guid?)null },
            new { PermissionId = TenantUpdatePermissionId, TenantId = SystemTenantId, Code = "tenants.update", Name = "Update Tenant", Description = "Permission to update tenants", Status = BaseStatus.Active, CreatedAt = DateTimeOffset.Parse("2024-01-01T00:00:00Z"), UpdatedAt = (DateTimeOffset?)null, CreatedBy = (Guid?)null, UpdatedBy = (Guid?)null },
            new { PermissionId = TenantDeletePermissionId, TenantId = SystemTenantId, Code = "tenants.delete", Name = "Delete Tenant", Description = "Permission to delete tenants", Status = BaseStatus.Active, CreatedAt = DateTimeOffset.Parse("2024-01-01T00:00:00Z"), UpdatedAt = (DateTimeOffset?)null, CreatedBy = (Guid?)null, UpdatedBy = (Guid?)null },

            // User permissions
            new { PermissionId = UserCreatePermissionId, TenantId = SystemTenantId, Code = "users.create", Name = "Create User", Description = "Permission to create users", Status = BaseStatus.Active, CreatedAt = DateTimeOffset.Parse("2024-01-01T00:00:00Z"), UpdatedAt = (DateTimeOffset?)null, CreatedBy = (Guid?)null, UpdatedBy = (Guid?)null },
            new { PermissionId = UserReadPermissionId, TenantId = SystemTenantId, Code = "users.read", Name = "Read User", Description = "Permission to read users", Status = BaseStatus.Active, CreatedAt = DateTimeOffset.Parse("2024-01-01T00:00:00Z"), UpdatedAt = (DateTimeOffset?)null, CreatedBy = (Guid?)null, UpdatedBy = (Guid?)null },
            new { PermissionId = UserUpdatePermissionId, TenantId = SystemTenantId, Code = "users.update", Name = "Update User", Description = "Permission to update users", Status = BaseStatus.Active, CreatedAt = DateTimeOffset.Parse("2024-01-01T00:00:00Z"), UpdatedAt = (DateTimeOffset?)null, CreatedBy = (Guid?)null, UpdatedBy = (Guid?)null },
            new { PermissionId = UserDeletePermissionId, TenantId = SystemTenantId, Code = "users.delete", Name = "Delete User", Description = "Permission to delete users", Status = BaseStatus.Active, CreatedAt = DateTimeOffset.Parse("2024-01-01T00:00:00Z"), UpdatedAt = (DateTimeOffset?)null, CreatedBy = (Guid?)null, UpdatedBy = (Guid?)null },

            // Group permissions
            new { PermissionId = GroupCreatePermissionId, TenantId = SystemTenantId, Code = "groups.create", Name = "Create Group", Description = "Permission to create groups", Status = BaseStatus.Active, CreatedAt = DateTimeOffset.Parse("2024-01-01T00:00:00Z"), UpdatedAt = (DateTimeOffset?)null, CreatedBy = (Guid?)null, UpdatedBy = (Guid?)null },
            new { PermissionId = GroupReadPermissionId, TenantId = SystemTenantId, Code = "groups.read", Name = "Read Group", Description = "Permission to read groups", Status = BaseStatus.Active, CreatedAt = DateTimeOffset.Parse("2024-01-01T00:00:00Z"), UpdatedAt = (DateTimeOffset?)null, CreatedBy = (Guid?)null, UpdatedBy = (Guid?)null },
            new { PermissionId = GroupUpdatePermissionId, TenantId = SystemTenantId, Code = "groups.update", Name = "Update Group", Description = "Permission to update groups", Status = BaseStatus.Active, CreatedAt = DateTimeOffset.Parse("2024-01-01T00:00:00Z"), UpdatedAt = (DateTimeOffset?)null, CreatedBy = (Guid?)null, UpdatedBy = (Guid?)null },
            new { PermissionId = GroupDeletePermissionId, TenantId = SystemTenantId, Code = "groups.delete", Name = "Delete Group", Description = "Permission to delete groups", Status = BaseStatus.Active, CreatedAt = DateTimeOffset.Parse("2024-01-01T00:00:00Z"), UpdatedAt = (DateTimeOffset?)null, CreatedBy = (Guid?)null, UpdatedBy = (Guid?)null },

            // Permission permissions
            new { PermissionId = PermissionCreatePermissionId, TenantId = SystemTenantId, Code = "permissions.create", Name = "Create Permission", Description = "Permission to create permissions", Status = BaseStatus.Active, CreatedAt = DateTimeOffset.Parse("2024-01-01T00:00:00Z"), UpdatedAt = (DateTimeOffset?)null, CreatedBy = (Guid?)null, UpdatedBy = (Guid?)null },
            new { PermissionId = PermissionReadPermissionId, TenantId = SystemTenantId, Code = "permissions.read", Name = "Read Permission", Description = "Permission to read permissions", Status = BaseStatus.Active, CreatedAt = DateTimeOffset.Parse("2024-01-01T00:00:00Z"), UpdatedAt = (DateTimeOffset?)null, CreatedBy = (Guid?)null, UpdatedBy = (Guid?)null },
            new { PermissionId = PermissionUpdatePermissionId, TenantId = SystemTenantId, Code = "permissions.update", Name = "Update Permission", Description = "Permission to update permissions", Status = BaseStatus.Active, CreatedAt = DateTimeOffset.Parse("2024-01-01T00:00:00Z"), UpdatedAt = (DateTimeOffset?)null, CreatedBy = (Guid?)null, UpdatedBy = (Guid?)null },
            new { PermissionId = PermissionDeletePermissionId, TenantId = SystemTenantId, Code = "permissions.delete", Name = "Delete Permission", Description = "Permission to delete permissions", Status = BaseStatus.Active, CreatedAt = DateTimeOffset.Parse("2024-01-01T00:00:00Z"), UpdatedAt = (DateTimeOffset?)null, CreatedBy = (Guid?)null, UpdatedBy = (Guid?)null }
        };

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

    private static void SeedGroupPermissions(ModelBuilder modelBuilder)
    {
        var groupPermissions = new[]
        {
            // Tenant permissions for Admin group
            new { GroupPermissionId = new Guid("00000000-0000-0000-0001-000000000001"), GroupId = AdminGroupId, PermissionId = TenantCreatePermissionId, TenantId = SystemTenantId, CreatedAt = DateTimeOffset.Parse("2024-01-01T00:00:00Z"), UpdatedAt = (DateTimeOffset?)null, CreatedBy = (Guid?)null, UpdatedBy = (Guid?)null },
            new { GroupPermissionId = new Guid("00000000-0000-0000-0001-000000000002"), GroupId = AdminGroupId, PermissionId = TenantReadPermissionId, TenantId = SystemTenantId, CreatedAt = DateTimeOffset.Parse("2024-01-01T00:00:00Z"), UpdatedAt = (DateTimeOffset?)null, CreatedBy = (Guid?)null, UpdatedBy = (Guid?)null },
            new { GroupPermissionId = new Guid("00000000-0000-0000-0001-000000000003"), GroupId = AdminGroupId, PermissionId = TenantUpdatePermissionId, TenantId = SystemTenantId, CreatedAt = DateTimeOffset.Parse("2024-01-01T00:00:00Z"), UpdatedAt = (DateTimeOffset?)null, CreatedBy = (Guid?)null, UpdatedBy = (Guid?)null },
            new { GroupPermissionId = new Guid("00000000-0000-0000-0001-000000000004"), GroupId = AdminGroupId, PermissionId = TenantDeletePermissionId, TenantId = SystemTenantId, CreatedAt = DateTimeOffset.Parse("2024-01-01T00:00:00Z"), UpdatedAt = (DateTimeOffset?)null, CreatedBy = (Guid?)null, UpdatedBy = (Guid?)null },

            // User permissions for Admin group
            new { GroupPermissionId = new Guid("00000000-0000-0000-0002-000000000001"), GroupId = AdminGroupId, PermissionId = UserCreatePermissionId, TenantId = SystemTenantId, CreatedAt = DateTimeOffset.Parse("2024-01-01T00:00:00Z"), UpdatedAt = (DateTimeOffset?)null, CreatedBy = (Guid?)null, UpdatedBy = (Guid?)null },
            new { GroupPermissionId = new Guid("00000000-0000-0000-0002-000000000002"), GroupId = AdminGroupId, PermissionId = UserReadPermissionId, TenantId = SystemTenantId, CreatedAt = DateTimeOffset.Parse("2024-01-01T00:00:00Z"), UpdatedAt = (DateTimeOffset?)null, CreatedBy = (Guid?)null, UpdatedBy = (Guid?)null },
            new { GroupPermissionId = new Guid("00000000-0000-0000-0002-000000000003"), GroupId = AdminGroupId, PermissionId = UserUpdatePermissionId, TenantId = SystemTenantId, CreatedAt = DateTimeOffset.Parse("2024-01-01T00:00:00Z"), UpdatedAt = (DateTimeOffset?)null, CreatedBy = (Guid?)null, UpdatedBy = (Guid?)null },
            new { GroupPermissionId = new Guid("00000000-0000-0000-0002-000000000004"), GroupId = AdminGroupId, PermissionId = UserDeletePermissionId, TenantId = SystemTenantId, CreatedAt = DateTimeOffset.Parse("2024-01-01T00:00:00Z"), UpdatedAt = (DateTimeOffset?)null, CreatedBy = (Guid?)null, UpdatedBy = (Guid?)null },

            // Group permissions for Admin group
            new { GroupPermissionId = new Guid("00000000-0000-0000-0003-000000000001"), GroupId = AdminGroupId, PermissionId = GroupCreatePermissionId, TenantId = SystemTenantId, CreatedAt = DateTimeOffset.Parse("2024-01-01T00:00:00Z"), UpdatedAt = (DateTimeOffset?)null, CreatedBy = (Guid?)null, UpdatedBy = (Guid?)null },
            new { GroupPermissionId = new Guid("00000000-0000-0000-0003-000000000002"), GroupId = AdminGroupId, PermissionId = GroupReadPermissionId, TenantId = SystemTenantId, CreatedAt = DateTimeOffset.Parse("2024-01-01T00:00:00Z"), UpdatedAt = (DateTimeOffset?)null, CreatedBy = (Guid?)null, UpdatedBy = (Guid?)null },
            new { GroupPermissionId = new Guid("00000000-0000-0000-0003-000000000003"), GroupId = AdminGroupId, PermissionId = GroupUpdatePermissionId, TenantId = SystemTenantId, CreatedAt = DateTimeOffset.Parse("2024-01-01T00:00:00Z"), UpdatedAt = (DateTimeOffset?)null, CreatedBy = (Guid?)null, UpdatedBy = (Guid?)null },
            new { GroupPermissionId = new Guid("00000000-0000-0000-0003-000000000004"), GroupId = AdminGroupId, PermissionId = GroupDeletePermissionId, TenantId = SystemTenantId, CreatedAt = DateTimeOffset.Parse("2024-01-01T00:00:00Z"), UpdatedAt = (DateTimeOffset?)null, CreatedBy = (Guid?)null, UpdatedBy = (Guid?)null },

            // Permission permissions for Admin group
            new { GroupPermissionId = new Guid("00000000-0000-0000-0004-000000000001"), GroupId = AdminGroupId, PermissionId = PermissionCreatePermissionId, TenantId = SystemTenantId, CreatedAt = DateTimeOffset.Parse("2024-01-01T00:00:00Z"), UpdatedAt = (DateTimeOffset?)null, CreatedBy = (Guid?)null, UpdatedBy = (Guid?)null },
            new { GroupPermissionId = new Guid("00000000-0000-0000-0004-000000000002"), GroupId = AdminGroupId, PermissionId = PermissionReadPermissionId, TenantId = SystemTenantId, CreatedAt = DateTimeOffset.Parse("2024-01-01T00:00:00Z"), UpdatedAt = (DateTimeOffset?)null, CreatedBy = (Guid?)null, UpdatedBy = (Guid?)null },
            new { GroupPermissionId = new Guid("00000000-0000-0000-0004-000000000003"), GroupId = AdminGroupId, PermissionId = PermissionUpdatePermissionId, TenantId = SystemTenantId, CreatedAt = DateTimeOffset.Parse("2024-01-01T00:00:00Z"), UpdatedAt = (DateTimeOffset?)null, CreatedBy = (Guid?)null, UpdatedBy = (Guid?)null },
            new { GroupPermissionId = new Guid("00000000-0000-0000-0004-000000000004"), GroupId = AdminGroupId, PermissionId = PermissionDeletePermissionId, TenantId = SystemTenantId, CreatedAt = DateTimeOffset.Parse("2024-01-01T00:00:00Z"), UpdatedAt = (DateTimeOffset?)null, CreatedBy = (Guid?)null, UpdatedBy = (Guid?)null }
        };

        modelBuilder.Entity<GroupPermission>().HasData(groupPermissions);
    }
}
