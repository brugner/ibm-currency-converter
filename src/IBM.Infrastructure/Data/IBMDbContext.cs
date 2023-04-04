using IBM.Domain.Entities;
using IBM.Infrastructure.EntitiesConfiguration;
using Microsoft.EntityFrameworkCore;

namespace IBM.Infrastructure.Data;

public class IBMDbContext : DbContext
{
    public DbSet<Rate> Rates { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

    public IBMDbContext(DbContextOptions<IBMDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new RateEntityConfiguration());
        modelBuilder.ApplyConfiguration(new TransactionEntityConfiguration());
    }
}