using IBM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IBM.Infrastructure.EntitiesConfiguration;

public class TransactionEntityConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Sku).IsRequired();
        builder.Property(x => x.Amount).IsRequired();
        builder.Property(x => x.Currency).IsRequired().HasMaxLength(3);

        builder.HasIndex(x => x.Sku).HasDatabaseName("IX_Transactions_Sku");
    }
}