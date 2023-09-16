using APIBaseTemplate.Datamodel.DbEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace APIBaseTemplate.Datamodel.Mapping
{
    public class AirportMap : IEntityTypeConfiguration<Airport>
    {
        public void Configure(EntityTypeBuilder<Airport> builder)
        {
            // Table
            builder.ToTable("Airports");

            // Indexes
            builder.HasKey(x => x.AirportId);
            builder.HasIndex(x => x.CityId).HasDatabaseName("IDX_AIRPORTS_CITY");

            // Columns
            builder.Property(p => p.AirportId).UseIdentityColumn();
            builder.Property(p => p.Code).HasMaxLength(4).IsRequired();
            builder.Property(p => p.Name).HasMaxLength(100).IsRequired();
            builder.Property(p => p.CityId).IsRequired();

            // Keys
            builder.HasOne(p => p.City)
                .WithMany()
                .HasForeignKey(p => p.CityId)
                .HasConstraintName("FK_AIRPORTS_CITY")
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
