using BSourceCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BSourceCore.Infrastructure.Persistence.Configurations;

public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("Permissions");

        builder.HasKey(p => p.PermissionId);

        builder.Property(p => p.Code)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(p => new { p.TenantId, p.Code })
            .IsUnique();

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Description)
            .HasMaxLength(500);

        builder.Property(p => p.Status)
            .IsRequired();

        // Audit fields
        builder.Property(p => p.CreatedAt).IsRequired();
        builder.Property(p => p.UpdatedAt);
        builder.Property(p => p.CreatedById);
        builder.Property(p => p.UpdatedById);

        // Relationships
        builder.HasMany(p => p.GroupPermissions)
            .WithOne(gp => gp.Permission)
            .HasForeignKey(gp => gp.PermissionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
