using Microsoft.EntityFrameworkCore;
using System.Reflection;

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
    }
}
