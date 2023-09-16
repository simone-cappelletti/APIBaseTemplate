using APIBaseTemplate.Datamodel.DbEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace APIBaseTemplate.Datamodel.Mapping
{
    public class CurrencyMap : IEntityTypeConfiguration<Currency>
    {
        public void Configure(EntityTypeBuilder<Currency> builder)
        {
            // Table
            builder.ToTable("Currencies");

            // Indexes
            builder.HasKey(x => x.CurrencyId);

            // Columns
            builder.Property(p => p.CurrencyId).UseIdentityColumn();
            builder.Property(p => p.Code).HasMaxLength(3).IsRequired();
            builder.Property(p => p.Name).HasMaxLength(30).IsRequired();
        }
    }
}
