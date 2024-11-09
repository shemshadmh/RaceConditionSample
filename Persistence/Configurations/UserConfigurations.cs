using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RaceConditionSample.Entities;

namespace RaceConditionSample.Persistence.Configurations;

public class UserConfigurations : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Email)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(e => e.Username)
            .HasColumnType("varchar")
            .HasMaxLength(100)
            .IsRequired();
        builder.HasIndex(e => e.Username).IsUnique();
    }
}