using APIBaseTemplate.Datamodel.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace APIBaseTemplate.Datamodel.Mapping
{
    public class AirlineMap : IEntityTypeConfiguration<Airline>
    {
        public void Configure(EntityTypeBuilder<Airline> builder)
        {
            // Table
            builder.ToTable("Airlines");

            // Indexes
            builder.HasKey(p => p.AirlineId);
            builder.HasIndex(p => p.RegionId).HasDatabaseName("IDX_AIRLINES_REGION");

            // Columns
            builder.Property(p => p.AirlineId).UseIdentityColumn();
            builder.Property(p => p.Code).HasMaxLength(4).IsRequired();
            builder.Property(p => p.Name).HasMaxLength(100).IsRequired();
            builder.Property(p => p.RegionId).IsRequired();

            // Keys
            builder.HasOne(p => p.Region)
                .WithMany()
                .HasForeignKey(P => P.RegionId)
                .HasConstraintName("FK_AIRLINES_REGION")
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
