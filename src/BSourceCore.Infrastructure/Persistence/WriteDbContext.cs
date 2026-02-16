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

    public WriteDbContext(
        DbContextOptions<WriteDbContext> options,
        ITenantContext tenantContext) : base(options)
    {
        _tenantContext = tenantContext;
    }

    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Group> Groups => Set<Group>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<UserGroup> UserGroups => Set<UserGroup>();
    public DbSet<GroupPermission> GroupPermissions => Set<GroupPermission>();
    public DbSet<PasswordReset> PasswordResets => Set<PasswordReset>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(WriteDbContext).Assembly);

        // Apply seed data
        SeedData.ApplySeedData(modelBuilder);

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
            if (entry.State == EntityState.Modified)
            {
                entry.Property(e => e.UpdatedAt).CurrentValue = DateTimeOffset.UtcNow;
            }
        }
    }
}
