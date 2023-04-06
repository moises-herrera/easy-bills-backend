using EasyBills.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EasyBills.Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id)
            .ValueGeneratedOnAdd();

        builder.Property(u => u.FirstName)
            .HasMaxLength(150);

        builder.Property(u => u.LastName)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(u => u.Email)
            .IsRequired();

        builder.Property(u => u.Password)
            .IsRequired();

        builder.Property(u => u.IsEmailVerified)
            .HasDefaultValue(false);
    }
}
