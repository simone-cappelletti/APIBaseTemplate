using APIBaseTemplate.Common;
using APIBaseTemplate.Datamodel;
using APIBaseTemplate.Repositories.DataContexts;
using APIBaseTemplate.Repositories.Repositories;
using APIBaseTemplate.Repositories.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace APIBaseTemplate.Extensions
{
    public static class Extensions
    {
        /// <summary>
        /// Add the base db context.
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="configuration">The db context configuration.</param>
        public static void AddAPIBaseTemplateDbContext(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddDbContext<APIBaseTemplateDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString(Constants.API_BASE_TEMPLATE_CONNECTIONSTRING_NAME));
            });

            serviceCollection.AddScoped<DataContext>(serviceProvider =>
            {
                var db = serviceProvider.GetService<APIBaseTemplateDbContext>();
                var result = new DataContext(db);
                return result;
            });

            serviceCollection.AddScoped<IDataContext>(serviceProvider => serviceProvider.GetService<DataContext>());
            serviceCollection.AddUnitOfWorkPattern();
        }

        private static void AddUnitOfWorkPattern(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IUnitOfWorkFactory, UnitOfWorkFactory>(serviceProvider =>
            {
                var factory = new UnitOfWorkFactory(
                    defaultFactory: () =>
                    {
                        var dataContext = serviceProvider.GetService<IDataContext>();
                        return new UnitOfWork(dataContext);
                    });
                return factory;
            });

            serviceCollection.AddTransient(typeof(IRepository<>), typeof(BaseRepository<>));
        }
    }
}
