﻿using EasyBills.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EasyBills.Infrastructure.Data.Configurations;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id)
            .ValueGeneratedOnAdd();

        builder.Property(a => a.Name)
            .HasMaxLength(80)
            .IsRequired();

        builder.Property(a => a.TypeAccount)
            .IsRequired();

        builder.Property(a => a.Balance)
            .HasPrecision(precision: 18, scale: 2)
            .IsRequired();
    }
}
