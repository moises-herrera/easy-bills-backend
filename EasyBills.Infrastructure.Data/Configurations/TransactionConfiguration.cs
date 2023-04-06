using EasyBills.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EasyBills.Infrastructure.Data.Configurations;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id)
            .ValueGeneratedOnAdd();

        builder.Property(t => t.Amount)
            .HasPrecision(precision: 18, scale: 2)
            .IsRequired();

        builder.Property(t => t.Description)
            .HasMaxLength(150);

        builder.Property(t => t.IsIncome)
            .IsRequired();
    }
}
