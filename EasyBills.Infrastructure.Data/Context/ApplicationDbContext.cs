using EasyBills.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace EasyBills.Infrastructure.Data.Context;

/// <summary>
/// Application DB context.
/// </summary>
public class ApplicationDbContext : DbContext
{
    /// <summary>
    /// Initialize a new instance of <see cref="ApplicationDbContext"/> class.
    /// </summary>
    /// <param name="options">Context options.</param>
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {

    }

    /// <summary>
    /// Configure global conventions.
    /// </summary>
    /// <param name="configurationBuilder">Config builder.</param>
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<DateTime>().HaveColumnType("date");
    }

    /// <summary>
    /// Apply configurations for the models.
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    /// <summary>
    /// Users table.
    /// </summary>
    public DbSet<User> Users { get; set; }

    /// <summary>
    /// Accounts table.
    /// </summary>
    public DbSet<Account> Accounts { get; set; }

    /// <summary>
    /// Categories table.
    /// </summary>
    public DbSet<Category> Categories { get; set; }

    /// <summary>
    /// Transactions table.
    /// </summary>
    public DbSet<Transaction> Transactions { get; set; }
}
