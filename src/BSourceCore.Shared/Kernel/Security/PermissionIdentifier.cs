namespace BSourceCore.Shared.Kernel.Security;

public class PermissionIdentifier
{
    public Guid Id { get; set; }
    public string Code { get; set; } = "";
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";

    protected PermissionIdentifier() { }

    public static List<PermissionIdentifier> GetList()
    {
        return
        [
            // Tenant
            new() { Id = TenantCreatePermissionId, Code = "tenants.create", Name = "Create Tenant", Description = "Permission to create tenants" },
            new() { Id = TenantReadPermissionId, Code = "tenants.read", Name = "Read Tenant", Description = "Permission to read tenants" },
            new() { Id = TenantUpdatePermissionId, Code = "tenants.update", Name = "Update Tenant", Description = "Permission to update tenants" },
            new() { Id = TenantDeletePermissionId, Code = "tenants.delete", Name = "Delete Tenant", Description = "Permission to delete tenants" },

            // User
            new() { Id = UserCreatePermissionId, Code = "users.create", Name = "Create User", Description = "Permission to create users" },
            new() { Id = UserReadPermissionId, Code = "users.read", Name = "Read User", Description = "Permission to read users" },
            new() { Id = UserUpdatePermissionId, Code = "users.update", Name = "Update User", Description = "Permission to update users" },
            new() { Id = UserDeletePermissionId, Code = "users.delete", Name = "Delete User", Description = "Permission to delete users" },

            // Group
            new() { Id = GroupCreatePermissionId, Code = "groups.create", Name = "Create Group", Description = "Permission to create groups" },
            new() { Id = GroupReadPermissionId, Code = "groups.read", Name = "Read Group", Description = "Permission to read groups" },
            new() { Id = GroupUpdatePermissionId, Code = "groups.update", Name = "Update Group", Description = "Permission to update groups" },
            new() { Id = GroupDeletePermissionId, Code = "groups.delete", Name = "Delete Group", Description = "Permission to delete groups" },

            // Permission
            new() { Id = PermissionCreatePermissionId, Code = "permissions.create", Name = "Create Permission", Description = "Permission to create permissions" },
            new() { Id = PermissionReadPermissionId, Code = "permissions.read", Name = "Read Permission", Description = "Permission to read permissions" },
            new() { Id = PermissionUpdatePermissionId, Code = "permissions.update", Name = "Update Permission", Description = "Permission to update permissions" },
            new() { Id = PermissionDeletePermissionId, Code = "permissions.delete", Name = "Delete Permission", Description = "Permission to delete permissions" }
        ];
    }

    #region Permission IDs

    // Tenant
    public static readonly Guid TenantCreatePermissionId = new("10000000-0000-0000-0000-000000000001");
    public static readonly Guid TenantReadPermissionId = new("10000000-0000-0000-0000-000000000002");
    public static readonly Guid TenantUpdatePermissionId = new("10000000-0000-0000-0000-000000000003");
    public static readonly Guid TenantDeletePermissionId = new("10000000-0000-0000-0000-000000000004");

    // User
    public static readonly Guid UserCreatePermissionId = new("20000000-0000-0000-0000-000000000001");
    public static readonly Guid UserReadPermissionId = new("20000000-0000-0000-0000-000000000002");
    public static readonly Guid UserUpdatePermissionId = new("20000000-0000-0000-0000-000000000003");
    public static readonly Guid UserDeletePermissionId = new("20000000-0000-0000-0000-000000000004");

    // Group
    public static readonly Guid GroupCreatePermissionId = new("30000000-0000-0000-0000-000000000001");
    public static readonly Guid GroupReadPermissionId = new("30000000-0000-0000-0000-000000000002");
    public static readonly Guid GroupUpdatePermissionId = new("30000000-0000-0000-0000-000000000003");
    public static readonly Guid GroupDeletePermissionId = new("30000000-0000-0000-0000-000000000004");

    // Permission
    public static readonly Guid PermissionCreatePermissionId = new("40000000-0000-0000-0000-000000000001");
    public static readonly Guid PermissionReadPermissionId = new("40000000-0000-0000-0000-000000000002");
    public static readonly Guid PermissionUpdatePermissionId = new("40000000-0000-0000-0000-000000000003");
    public static readonly Guid PermissionDeletePermissionId = new("40000000-0000-0000-0000-000000000004");

    #endregion
}