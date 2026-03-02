using BSourceCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BSourceCore.Infrastructure.Persistence.Configurations;

public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
{
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder.HasOne(x => x.CreatedBy)
         .WithMany()
         .HasForeignKey(x => x.CreatedById)
         .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.UpdatedBy)
         .WithMany()
         .HasForeignKey(x => x.UpdatedById)
         .OnDelete(DeleteBehavior.Restrict);

        // builder.HasMany(x => x.Users)
        //  .WithMany(x => x.Tenants) // precisa existir em User; se não existir, use .WithMany()
        //  .UsingEntity<Dictionary<string, object>>(
        //     "TenantUser",
        //     j => j.HasOne<User>().WithMany().HasForeignKey("UserId"),
        //     j => j.HasOne<Tenant>().WithMany().HasForeignKey("TenantId")
        //  );
    }
}
