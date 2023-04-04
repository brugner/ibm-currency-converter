using IBM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IBM.Infrastructure.EntitiesConfiguration;

public class RateEntityConfiguration : IEntityTypeConfiguration<Rate>
{
    public void Configure(EntityTypeBuilder<Rate> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.From).IsRequired().HasMaxLength(3);
        builder.Property(x => x.To).IsRequired().HasMaxLength(3);
        builder.Property(x => x.Value).IsRequired();

        builder.HasIndex(x => new { x.From, x.To }).IsUnique().HasDatabaseName("IX_Rates_From_To");
    }
}