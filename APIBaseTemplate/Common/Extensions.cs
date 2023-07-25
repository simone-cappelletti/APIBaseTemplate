using APIBaseTemplate.Datamodel;
using APIBaseTemplate.Repositories.DataContexts;
using APIBaseTemplate.Repositories;
using APIBaseTemplate.Repositories.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text;

namespace APIBaseTemplate.Common
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

            serviceCollection.AddScoped(serviceProvider =>
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

        /// <summary>
        /// Autogenerates a comment that contains all valid values of the enum
        /// </summary>
        /// <typeparam name="TEnum">enum sul quale generare il commento</typeparam>
        /// <param name="propertyBuilder"></param>
        /// <returns></returns>
        public static PropertyBuilder<TEnum> HasEnumComment<TEnum>(this PropertyBuilder<TEnum> propertyBuilder) where TEnum : Enum
        {
            var comment = string.Empty;
            var enumType = typeof(TEnum);
            var enumName = enumType.Name;
            var enumValues = Enum.GetNames(enumType);
            var isFlagEnum = Attribute.GetCustomAttribute(enumType, typeof(FlagsAttribute)) != null;

            var sb = new StringBuilder();

            // MyEnum flags (Flag0 = 0, Flag1 = 1, Flag2 = 2, Flag3 = 4)
            // MyEnum values (Value0 = 0, Value1 = 1, Value2 = 2, Value3 = 3)
            sb.Append(enumName);

            // flag value -> (flag enum) in OR
            // distinct value -> (regular enum)
            sb.Append(isFlagEnum ? " flags (" : " values (");

            for (int i = 0; i < enumValues.Length; i++)
            {
                var nameValue = enumValues[i];
                var intValue = (int)Enum.Parse(enumType, nameValue);

                sb.Append($"{nameValue} = {intValue}");

                if (i < enumValues.Length - 1)
                    sb.Append(", ");
            }

            sb.Append(')');

            comment = sb.ToString();
            propertyBuilder.HasComment(comment);

            return propertyBuilder;
        }
    }
}
