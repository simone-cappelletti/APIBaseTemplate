using APIBaseTemplate.Datamodel.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace APIBaseTemplate.Datamodel.Mapping
{
    public class FligthMap : IEntityTypeConfiguration<Fligth>
    {
        public void Configure(EntityTypeBuilder<Fligth> builder)
        {
            // Table
            builder.ToTable("Fligths");

            // Indexes
            builder.HasKey(p => p.FligthId);
            builder.HasIndex(p => p.AirlineId).HasDatabaseName("IDX_FLIGHTS_AIRLINE");
            builder.HasIndex(p => p.DepartureAirportId).HasDatabaseName("IDX_FLIGHTS_DEPARTURE_AIRPORT");
            builder.HasIndex(p => p.ArrivalAirportId).HasDatabaseName("IDX_FLIGHTS_ARRIVAL_AIRPORT");

            // Columns
            builder.Property(p => p.FligthId).UseIdentityColumn();
            builder.Property(p => p.Code).HasMaxLength(4).IsRequired();
            builder.Property(p => p.AirlineId).IsRequired();
            builder.Property(p => p.DepartureAirportId).IsRequired();
            builder.Property(p => p.ArrivalAirportId).IsRequired();
            builder.Property(p => p.DepartureTime).IsRequired();
            builder.Property(p => p.ArrivalTime).IsRequired();
            builder.Property(p => p.Gate).IsRequired();
            builder.Property(p => p.IsDeleted).IsRequired();

            // Keys
            builder.HasOne(p => p.Airline)
                .WithMany()
                .HasForeignKey(p => p.AirlineId)
                .HasConstraintName("FK_FLIGHTS_AIRLINE")
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(p => p.DepartureAirport)
                .WithMany()
                .HasForeignKey(p => p.DepartureAirportId)
                .HasConstraintName("FK_FLIGHTS_DEPARTURE_AIPORT")
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(p => p.ArrivalAirport)
                .WithMany()
                .HasForeignKey(p => p.ArrivalAirportId)
                .HasConstraintName("FK_FLIGHTS_ARRIVAL_AIRPORT")
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
