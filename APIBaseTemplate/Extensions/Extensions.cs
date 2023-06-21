using APIBaseTemplate.Common;
using APIBaseTemplate.Datamodel;
using Microsoft.EntityFrameworkCore;

namespace APIBaseTemplate.Extensions
{
    public static class Extensions
    {
        public static void AddAPIBaseTemplateDbContext(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddDbContext<APIBaseTemplateDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString(Constants.API_BASE_TEMPLATE_CONNECTIONSTRING_NAME));
            });
        }
    }
}
