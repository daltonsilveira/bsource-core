using BSourceCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BSourceCore.Infrastructure.Persistence.Configurations;

public class UserGroupConfiguration : IEntityTypeConfiguration<UserGroup>
{
    public void Configure(EntityTypeBuilder<UserGroup> builder)
    {
        builder.ToTable("UserGroups");

        builder.HasKey(ug => ug.UserGroupId);

        builder.HasIndex(ug => new { ug.UserId, ug.GroupId })
            .IsUnique();

        // Audit fields
        builder.Property(ug => ug.CreatedAt).IsRequired();
        builder.Property(ug => ug.UpdatedAt);
        builder.Property(ug => ug.CreatedById);
        builder.Property(ug => ug.UpdatedById);
    }
}
