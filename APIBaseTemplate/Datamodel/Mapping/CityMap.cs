using APIBaseTemplate.Datamodel.DbEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace APIBaseTemplate.Datamodel.Mapping
{
    public class CityMap : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            // Table
            builder.ToTable("Cities");

            // Indexes
            builder.HasKey(x => x.CityId);
            builder.HasIndex(x => x.RegionId).HasDatabaseName("IDX_CITIES_REGION");

            // Columns
            builder.Property(p => p.CityId).UseIdentityColumn();
            builder.Property(p => p.Name).HasMaxLength(100).IsRequired();
            builder.Property(p => p.RegionId).IsRequired();

            // Keys
            builder.HasOne(p => p.Region)
                .WithMany()
                .HasForeignKey(p => p.RegionId)
                .HasConstraintName("FK_CITIES_REGION")
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
