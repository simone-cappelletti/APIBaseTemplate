using APIBaseTemplate.Common;
using APIBaseTemplate.Datamodel.DbEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace APIBaseTemplate.Datamodel.Mapping
{
    public class FligthServiceMap : IEntityTypeConfiguration<FligthService>
    {
        public void Configure(EntityTypeBuilder<FligthService> builder)
        {
            // Table
            builder.ToTable("FligthServices");

            // Indexes
            builder.HasKey(x => x.FligthServiceId);
            builder.HasIndex(x => x.CurrencyId).HasDatabaseName("IDX_FLIGTHSERVICES_CURRENCY");
            builder.HasIndex(x => x.FligthId).HasDatabaseName("IDX_FLIGTHSERVICES_FLIGTH");

            // Columns
            builder.Property(p => p.FligthServiceId).UseIdentityColumn();
            builder.Property(p => p.PriceType).HasEnumComment().HasConversion<string>().IsRequired();
            builder.Property(p => p.Amout).IsRequired();
            builder.Property(p => p.CurrencyId).IsRequired();
            builder.Property(p => p.FligthId).IsRequired();

            // Keys
            builder.HasOne(p => p.Currency)
                .WithMany()
                .HasForeignKey(p => p.CurrencyId)
                .HasConstraintName("FK_FLIGTHSERVICES_CURRENCY")
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(p => p.Fligth)
                .WithMany(p => p.FligthServices)
                .HasForeignKey(p => p.FligthId)
                .HasConstraintName("FK_FLIGTHSERVICES_FLIGTH")
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
