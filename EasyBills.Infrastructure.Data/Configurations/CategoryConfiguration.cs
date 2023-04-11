using EasyBills.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EasyBills.Infrastructure.Data.Configurations;

/// <summary>
/// Category entity configuration.
/// </summary>
public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    /// <summary>
    /// Configure category entity.
    /// </summary>
    /// <param name="builder">Category builder.</param>
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .ValueGeneratedOnAdd();

        builder.Property(c => c.Name)
            .HasMaxLength(80)
            .IsRequired();

        builder.Property(c => c.Description)
            .HasMaxLength(150);
    }
}
