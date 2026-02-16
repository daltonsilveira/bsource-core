using BSourceCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BSourceCore.Infrastructure.Persistence.Configurations;

public class GroupConfiguration : IEntityTypeConfiguration<Group>
{
    public void Configure(EntityTypeBuilder<Group> builder)
    {
        builder.ToTable("Groups");

        builder.HasKey(g => g.GroupId);

        builder.Property(g => g.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasIndex(g => new { g.TenantId, g.Name })
            .IsUnique();

        builder.Property(g => g.Description)
            .HasMaxLength(500);

        builder.Property(g => g.Status)
            .IsRequired();

        // Audit fields
        builder.Property(g => g.CreatedAt).IsRequired();
        builder.Property(g => g.UpdatedAt);
        builder.Property(g => g.CreatedById);
        builder.Property(g => g.UpdatedById);

        // Relationships
        builder.HasMany(g => g.UserGroups)
            .WithOne(ug => ug.Group)
            .HasForeignKey(ug => ug.GroupId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(g => g.GroupPermissions)
            .WithOne(gp => gp.Group)
            .HasForeignKey(gp => gp.GroupId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
