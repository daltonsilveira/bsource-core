using System.Linq.Expressions;
using BSourceCore.Domain.Entities;
using BSourceCore.Domain.Interfaces;
using BSourceCore.Infrastructure.Persistence.Seed;
using BSourceCore.Shared.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace BSourceCore.Infrastructure.Persistence;

public class WriteDbContext : DbContext
{
    private readonly ITenantContext _tenantContext;
    private readonly IUserContext _userContext;

    public WriteDbContext(
        DbContextOptions<WriteDbContext> options,
        IUserContext userContext,
        ITenantContext tenantContext) : base(options)
    {
        _tenantContext = tenantContext;
        _userContext = userContext;
    }

    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Group> Groups => Set<Group>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<UserGroup> UserGroups => Set<UserGroup>();
    public DbSet<GroupPermission> GroupPermissions => Set<GroupPermission>();
    public DbSet<PasswordReset> PasswordResets => Set<PasswordReset>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<NotificationRecipient> NotificationRecipients => Set<NotificationRecipient>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(WriteDbContext).Assembly);

        // Apply seed data
        SeedData.ApplySeedData(modelBuilder);

        var cascadeFKs = modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior.Equals(DeleteBehavior.Cascade));

        foreach (var fk in cascadeFKs)
        {
            fk.DeleteBehavior = DeleteBehavior.Restrict;
        }

        // Apply filter to entities implementing the interface
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (_tenantContext.TenantId.HasValue
            && typeof(ITenantEntity).IsAssignableFrom(entityType.ClrType))
            {
                modelBuilder.Entity(entityType.ClrType)
                    .HasQueryFilter(BuildTenantFilter(entityType.ClrType));
            }
        }
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateAuditFields();
        SetTenantId();
        return base.SaveChangesAsync(cancellationToken);
    }

    private LambdaExpression BuildTenantFilter(Type entityType)
    {
        var parameter = Expression.Parameter(entityType, "e");
        var property = Expression.Property(parameter, nameof(ITenantEntity.TenantId));
        var tenantId = Expression.Constant(_tenantContext.TenantId);
        var body = Expression.Equal(property, tenantId);
        return Expression.Lambda(body, parameter);
    }

    private void SetTenantId()
    {
        if (!_tenantContext.TenantId.HasValue) return;
        foreach (var entry in ChangeTracker.Entries<ITenantEntity>())
        {
            if (entry.State == EntityState.Added && entry.Entity.TenantId == Guid.Empty)
            {
                entry.Entity.TenantId = _tenantContext.TenantId.Value;
            }
        }
    }

    private void UpdateAuditFields()
    {
        var entries = ChangeTracker.Entries<AuditEntity>();

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Property(e => e.CreatedAt).CurrentValue = DateTimeOffset.UtcNow;
                if (entry.Entity.CreatedById is null && _userContext.UserId.HasValue)
                {
                    entry.Property(e => e.CreatedById).CurrentValue = _userContext.UserId;
                }
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Property(e => e.UpdatedAt).CurrentValue = DateTimeOffset.UtcNow;
                if (entry.Entity.UpdatedById is null && _userContext.UserId.HasValue)
                {
                    entry.Property(e => e.UpdatedById).CurrentValue = _userContext.UserId;
                }
            }
        }
    }
}
