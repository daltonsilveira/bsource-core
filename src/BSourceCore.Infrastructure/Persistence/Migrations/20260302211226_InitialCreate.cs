using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BSourceCore.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GroupPermissions",
                columns: table => new
                {
                    GroupPermissionId = table.Column<Guid>(type: "uuid", nullable: false),
                    GroupId = table.Column<Guid>(type: "uuid", nullable: false),
                    PermissionId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedById = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupPermissions", x => x.GroupPermissionId);
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    GroupId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedById = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.GroupId);
                });

            migrationBuilder.CreateTable(
                name: "NotificationRecipients",
                columns: table => new
                {
                    NotificationRecipientId = table.Column<Guid>(type: "uuid", nullable: false),
                    NotificationId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    WasRead = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedById = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationRecipients", x => x.NotificationRecipientId);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    NotificationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Message = table.Column<string>(type: "character varying(8000)", maxLength: 8000, nullable: false),
                    Data = table.Column<string>(type: "character varying(8000)", maxLength: 8000, nullable: false),
                    CreatedById = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedById = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.NotificationId);
                });

            migrationBuilder.CreateTable(
                name: "PasswordResets",
                columns: table => new
                {
                    PasswordResetId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Token = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ExpiresAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedById = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasswordResets", x => x.PasswordResetId);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    PermissionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedById = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.PermissionId);
                });

            migrationBuilder.CreateTable(
                name: "Tenants",
                columns: table => new
                {
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Slug = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedById = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenants", x => x.TenantId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Login = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    PasswordHash = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    IsFirstAccess = table.Column<bool>(type: "boolean", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedById = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Users_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "TenantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_Users_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "UserGroups",
                columns: table => new
                {
                    UserGroupId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    GroupId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedById = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGroups", x => x.UserGroupId);
                    table.ForeignKey(
                        name: "FK_UserGroups_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserGroups_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "TenantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserGroups_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_UserGroups_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_UserGroups_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Tenants",
                columns: new[] { "TenantId", "CreatedAt", "CreatedById", "Description", "Name", "Slug", "Status", "UpdatedAt", "UpdatedById" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000001"), new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "System tenant for platform administration", "System", "system", 1, null, null });

            migrationBuilder.InsertData(
                table: "Groups",
                columns: new[] { "GroupId", "CreatedAt", "CreatedById", "Description", "Name", "Status", "TenantId", "UpdatedAt", "UpdatedById" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000003"), new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Full system administrators with all permissions", "Administrators", 1, new Guid("00000000-0000-0000-0000-000000000001"), null, null });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "PermissionId", "Code", "CreatedAt", "CreatedById", "Description", "Name", "Status", "TenantId", "UpdatedAt", "UpdatedById" },
                values: new object[,]
                {
                    { new Guid("10000000-0000-0000-0000-000000000001"), "tenants.create", new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Permission to create tenants", "Create Tenant", 1, new Guid("00000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("10000000-0000-0000-0000-000000000002"), "tenants.read", new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Permission to read tenants", "Read Tenant", 1, new Guid("00000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("10000000-0000-0000-0000-000000000003"), "tenants.update", new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Permission to update tenants", "Update Tenant", 1, new Guid("00000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("10000000-0000-0000-0000-000000000004"), "tenants.delete", new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Permission to delete tenants", "Delete Tenant", 1, new Guid("00000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("20000000-0000-0000-0000-000000000001"), "users.create", new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Permission to create users", "Create User", 1, new Guid("00000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("20000000-0000-0000-0000-000000000002"), "users.read", new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Permission to read users", "Read User", 1, new Guid("00000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("20000000-0000-0000-0000-000000000003"), "users.update", new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Permission to update users", "Update User", 1, new Guid("00000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("20000000-0000-0000-0000-000000000004"), "users.delete", new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Permission to delete users", "Delete User", 1, new Guid("00000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("30000000-0000-0000-0000-000000000001"), "groups.create", new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Permission to create groups", "Create Group", 1, new Guid("00000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("30000000-0000-0000-0000-000000000002"), "groups.read", new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Permission to read groups", "Read Group", 1, new Guid("00000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("30000000-0000-0000-0000-000000000003"), "groups.update", new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Permission to update groups", "Update Group", 1, new Guid("00000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("30000000-0000-0000-0000-000000000004"), "groups.delete", new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Permission to delete groups", "Delete Group", 1, new Guid("00000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("40000000-0000-0000-0000-000000000001"), "permissions.create", new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Permission to create permissions", "Create Permission", 1, new Guid("00000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("40000000-0000-0000-0000-000000000002"), "permissions.read", new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Permission to read permissions", "Read Permission", 1, new Guid("00000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("40000000-0000-0000-0000-000000000003"), "permissions.update", new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Permission to update permissions", "Update Permission", 1, new Guid("00000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("40000000-0000-0000-0000-000000000004"), "permissions.delete", new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Permission to delete permissions", "Delete Permission", 1, new Guid("00000000-0000-0000-0000-000000000001"), null, null }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "CreatedAt", "CreatedById", "Email", "IsFirstAccess", "Login", "Name", "PasswordHash", "Status", "TenantId", "UpdatedAt", "UpdatedById" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000002"), new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "admin@bsource.local", false, "admin", "System Administrator", "eFhNT1RYRVZ2NU9xUXpNbQ==.W/0eNJHDWZax1rVjZV/0v/EfAgg7Rywvx+VnEtH8bys=", 1, new Guid("00000000-0000-0000-0000-000000000001"), null, null });

            migrationBuilder.InsertData(
                table: "GroupPermissions",
                columns: new[] { "GroupPermissionId", "CreatedAt", "CreatedById", "GroupId", "PermissionId", "TenantId", "UpdatedAt", "UpdatedById" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0001-000000000001"), new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, new Guid("00000000-0000-0000-0000-000000000003"), new Guid("10000000-0000-0000-0000-000000000001"), new Guid("00000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("00000000-0000-0000-0001-000000000002"), new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, new Guid("00000000-0000-0000-0000-000000000003"), new Guid("10000000-0000-0000-0000-000000000002"), new Guid("00000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("00000000-0000-0000-0001-000000000003"), new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, new Guid("00000000-0000-0000-0000-000000000003"), new Guid("10000000-0000-0000-0000-000000000003"), new Guid("00000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("00000000-0000-0000-0001-000000000004"), new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, new Guid("00000000-0000-0000-0000-000000000003"), new Guid("10000000-0000-0000-0000-000000000004"), new Guid("00000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("00000000-0000-0000-0002-000000000001"), new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, new Guid("00000000-0000-0000-0000-000000000003"), new Guid("20000000-0000-0000-0000-000000000001"), new Guid("00000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("00000000-0000-0000-0002-000000000002"), new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, new Guid("00000000-0000-0000-0000-000000000003"), new Guid("20000000-0000-0000-0000-000000000002"), new Guid("00000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("00000000-0000-0000-0002-000000000003"), new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, new Guid("00000000-0000-0000-0000-000000000003"), new Guid("20000000-0000-0000-0000-000000000003"), new Guid("00000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("00000000-0000-0000-0002-000000000004"), new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, new Guid("00000000-0000-0000-0000-000000000003"), new Guid("20000000-0000-0000-0000-000000000004"), new Guid("00000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("00000000-0000-0000-0003-000000000001"), new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, new Guid("00000000-0000-0000-0000-000000000003"), new Guid("30000000-0000-0000-0000-000000000001"), new Guid("00000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("00000000-0000-0000-0003-000000000002"), new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, new Guid("00000000-0000-0000-0000-000000000003"), new Guid("30000000-0000-0000-0000-000000000002"), new Guid("00000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("00000000-0000-0000-0003-000000000003"), new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, new Guid("00000000-0000-0000-0000-000000000003"), new Guid("30000000-0000-0000-0000-000000000003"), new Guid("00000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("00000000-0000-0000-0003-000000000004"), new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, new Guid("00000000-0000-0000-0000-000000000003"), new Guid("30000000-0000-0000-0000-000000000004"), new Guid("00000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("00000000-0000-0000-0004-000000000001"), new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, new Guid("00000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000001"), new Guid("00000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("00000000-0000-0000-0004-000000000002"), new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, new Guid("00000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000002"), new Guid("00000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("00000000-0000-0000-0004-000000000003"), new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, new Guid("00000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000003"), new Guid("00000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("00000000-0000-0000-0004-000000000004"), new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, new Guid("00000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000004"), new Guid("00000000-0000-0000-0000-000000000001"), null, null }
                });

            migrationBuilder.InsertData(
                table: "UserGroups",
                columns: new[] { "UserGroupId", "CreatedAt", "CreatedById", "GroupId", "TenantId", "UpdatedAt", "UpdatedById", "UserId" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000100"), new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, new Guid("00000000-0000-0000-0000-000000000003"), new Guid("00000000-0000-0000-0000-000000000001"), null, null, new Guid("00000000-0000-0000-0000-000000000002") });

            migrationBuilder.CreateIndex(
                name: "IX_GroupPermissions_CreatedById",
                table: "GroupPermissions",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_GroupPermissions_GroupId",
                table: "GroupPermissions",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupPermissions_PermissionId",
                table: "GroupPermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupPermissions_TenantId",
                table: "GroupPermissions",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupPermissions_UpdatedById",
                table: "GroupPermissions",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_CreatedById",
                table: "Groups",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_TenantId",
                table: "Groups",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_UpdatedById",
                table: "Groups",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationRecipients_CreatedById",
                table: "NotificationRecipients",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationRecipients_NotificationId",
                table: "NotificationRecipients",
                column: "NotificationId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationRecipients_TenantId",
                table: "NotificationRecipients",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationRecipients_UpdatedById",
                table: "NotificationRecipients",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationRecipients_UserId",
                table: "NotificationRecipients",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_CreatedById",
                table: "Notifications",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_TenantId",
                table: "Notifications",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UpdatedById",
                table: "Notifications",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PasswordResets_CreatedById",
                table: "PasswordResets",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PasswordResets_TenantId",
                table: "PasswordResets",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_PasswordResets_UpdatedById",
                table: "PasswordResets",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PasswordResets_UserId",
                table: "PasswordResets",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_CreatedById",
                table: "Permissions",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_TenantId",
                table: "Permissions",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_UpdatedById",
                table: "Permissions",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_CreatedById",
                table: "Tenants",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_UpdatedById",
                table: "Tenants",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroups_CreatedById",
                table: "UserGroups",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroups_GroupId",
                table: "UserGroups",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroups_TenantId",
                table: "UserGroups",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroups_UpdatedById",
                table: "UserGroups",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroups_UserId",
                table: "UserGroups",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CreatedById",
                table: "Users",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Users_TenantId_Email",
                table: "Users",
                columns: new[] { "TenantId", "Email" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_UpdatedById",
                table: "Users",
                column: "UpdatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupPermissions_Groups_GroupId",
                table: "GroupPermissions",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "GroupId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupPermissions_Permissions_PermissionId",
                table: "GroupPermissions",
                column: "PermissionId",
                principalTable: "Permissions",
                principalColumn: "PermissionId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupPermissions_Tenants_TenantId",
                table: "GroupPermissions",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "TenantId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupPermissions_Users_CreatedById",
                table: "GroupPermissions",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupPermissions_Users_UpdatedById",
                table: "GroupPermissions",
                column: "UpdatedById",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Tenants_TenantId",
                table: "Groups",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "TenantId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Users_CreatedById",
                table: "Groups",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Users_UpdatedById",
                table: "Groups",
                column: "UpdatedById",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationRecipients_Notifications_NotificationId",
                table: "NotificationRecipients",
                column: "NotificationId",
                principalTable: "Notifications",
                principalColumn: "NotificationId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationRecipients_Tenants_TenantId",
                table: "NotificationRecipients",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "TenantId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationRecipients_Users_CreatedById",
                table: "NotificationRecipients",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationRecipients_Users_UpdatedById",
                table: "NotificationRecipients",
                column: "UpdatedById",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationRecipients_Users_UserId",
                table: "NotificationRecipients",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Tenants_TenantId",
                table: "Notifications",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "TenantId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Users_CreatedById",
                table: "Notifications",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Users_UpdatedById",
                table: "Notifications",
                column: "UpdatedById",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PasswordResets_Tenants_TenantId",
                table: "PasswordResets",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "TenantId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PasswordResets_Users_CreatedById",
                table: "PasswordResets",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PasswordResets_Users_UpdatedById",
                table: "PasswordResets",
                column: "UpdatedById",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PasswordResets_Users_UserId",
                table: "PasswordResets",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Permissions_Tenants_TenantId",
                table: "Permissions",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "TenantId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Permissions_Users_CreatedById",
                table: "Permissions",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Permissions_Users_UpdatedById",
                table: "Permissions",
                column: "UpdatedById",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tenants_Users_CreatedById",
                table: "Tenants",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tenants_Users_UpdatedById",
                table: "Tenants",
                column: "UpdatedById",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Tenants_TenantId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "GroupPermissions");

            migrationBuilder.DropTable(
                name: "NotificationRecipients");

            migrationBuilder.DropTable(
                name: "PasswordResets");

            migrationBuilder.DropTable(
                name: "UserGroups");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "Tenants");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
