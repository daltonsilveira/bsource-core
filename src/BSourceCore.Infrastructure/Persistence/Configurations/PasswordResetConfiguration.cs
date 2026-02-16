using BSourceCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BSourceCore.Infrastructure.Persistence.Configurations;

public class PasswordResetConfiguration : IEntityTypeConfiguration<PasswordReset>
{
    public void Configure(EntityTypeBuilder<PasswordReset> builder)
    {
        builder.ToTable("PasswordResets");

        builder.HasKey(pr => pr.PasswordResetId);

        builder.Property(pr => pr.Token)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(pr => pr.Token)
            .IsUnique();

        builder.Property(pr => pr.ExpiresAt)
            .IsRequired();

        builder.Property(pr => pr.Status)
            .IsRequired();

        // Audit fields
        builder.Property(pr => pr.CreatedAt).IsRequired();
        builder.Property(pr => pr.UpdatedAt);
        builder.Property(pr => pr.CreatedById);
        builder.Property(pr => pr.UpdatedById);

        // Relationships
        builder.HasOne(pr => pr.User)
            .WithMany()
            .HasForeignKey(pr => pr.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
