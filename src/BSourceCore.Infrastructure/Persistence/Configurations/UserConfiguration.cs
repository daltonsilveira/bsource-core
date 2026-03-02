using BSourceCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BSourceCore.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasIndex(u => new { u.TenantId, u.Email })
            .IsUnique();

        builder.HasMany(x => x.UserGroups)
         .WithOne(x => x.User)
         .OnDelete(DeleteBehavior.Restrict);
    }
}
