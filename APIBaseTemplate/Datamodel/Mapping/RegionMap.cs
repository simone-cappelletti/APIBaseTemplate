using APIBaseTemplate.Datamodel.DbEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace APIBaseTemplate.Datamodel.Mapping
{
    public class RegionMap : IEntityTypeConfiguration<Region>
    {
        public void Configure(EntityTypeBuilder<Region> builder)
        {
            // Table
            builder.ToTable("Regions");

            // Indexes
            builder.HasKey(x => x.RegionId);

            // Columns
            builder.Property(p => p.RegionId).UseIdentityColumn();
            builder.Property(p => p.Name).HasMaxLength(100).IsRequired();
        }
    }
}
