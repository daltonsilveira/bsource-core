using BSourceCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BSourceCore.Infrastructure.Persistence.Configurations;

public class GroupPermissionConfiguration : IEntityTypeConfiguration<GroupPermission>
{
    public void Configure(EntityTypeBuilder<GroupPermission> builder)
    {
        builder.ToTable("GroupPermissions");

        builder.HasKey(gp => gp.GroupPermissionId);

        builder.HasIndex(gp => new { gp.GroupId, gp.PermissionId })
            .IsUnique();

        // Audit fields
        builder.Property(gp => gp.CreatedAt).IsRequired();
        builder.Property(gp => gp.UpdatedAt);
        builder.Property(gp => gp.CreatedById);
        builder.Property(gp => gp.UpdatedById);
    }
}
