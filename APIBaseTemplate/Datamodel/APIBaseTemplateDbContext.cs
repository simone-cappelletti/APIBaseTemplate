using APIBaseTemplate.Datamodel.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace APIBaseTemplate.Datamodel
{
    public class APIBaseTemplateDbContext : DbContext
    {
        public APIBaseTemplateDbContext(DbContextOptions options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(APIBaseTemplateDbContext).Assembly);
        }

        /// <summary>
        /// SaveChanges method with logical delete support
        /// </summary>
        /// <returns></returns>
        public override int SaveChanges()
        {
            var deletedEntries = ChangeTracker
                .Entries()
                .Where(e => e.State == EntityState.Deleted && typeof(IDeletableEntity).IsAssignableFrom(e.Entity.GetType()))
                .ToList();

            deletedEntries.ForEach(entry =>
            {
                ((IDeletableEntity)entry.Entity).IsDeleted = true;

                entry.State = EntityState.Modified;
            });

            return base.SaveChanges();
        }
    }
}
