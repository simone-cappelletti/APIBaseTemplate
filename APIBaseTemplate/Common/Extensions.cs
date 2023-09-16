using APIBaseTemplate.Datamodel;
using APIBaseTemplate.Repositories;
using APIBaseTemplate.Repositories.DataContexts;
using APIBaseTemplate.Repositories.UnitOfWork;
using APIBaseTemplate.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Runtime.CompilerServices;
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

    public static class VerifyExtensions
    {
        /// <summary>
        /// Is's a <see cref="DateTimeKind.Unspecified"/> <see cref="DateTime"/>.
        /// </summary>
        /// <param name="verify">the verify</param>
        /// <param name="value">value to be controlled</param>
        /// <param name="valueMessage">value message</param>
        /// <exception cref="ArgumentException"></exception>
        public static void UnspecifiedKind(this IVerify verify, DateTime value, [CallerArgumentExpression("value")] string? valueMessage = null)
        {
            if (verify is null)
            {
                throw new ArgumentNullException(nameof(verify));
            }
            verify.Satisfied(value.Kind == DateTimeKind.Unspecified,
                assertValueMessage => new ArgumentException(
                verify.NotOperatorEnabled ?
                    $"Parameter should be not DateTimeKind.Unspecified. Condition [!({assertValueMessage})] not satisfied" :
                    $"Parameter should be DateTimeKind.Unspecified. Condition [{assertValueMessage}] not satisfied",
                valueMessage)
                );
        }

        /// <summary>
        /// Is's a <see cref="DateTimeKind.Utc"/> <see cref="DateTime"/>.
        /// </summary>
        /// <param name="verify">the verify</param>
        /// <param name="value">value to be controlled</param>
        /// <param name="valueMessage">value message</param>
        /// <exception cref="ArgumentException"></exception>
        public static void UtcKind(this IVerify verify, DateTime value, [CallerArgumentExpression("value")] string? valueMessage = null)
        {
            if (verify is null)
            {
                throw new ArgumentNullException(nameof(verify));
            }
            verify.Satisfied(value.Kind == DateTimeKind.Utc,
                assertValueMessage => new ArgumentException(
                verify.NotOperatorEnabled ?
                    $"Parameter should be not DateTimeKind.Utc. Condition [!({assertValueMessage})] not satisfied" :
                    $"Parameter should be DateTimeKind.Utc. Condition [{assertValueMessage}] not satisfied",
                valueMessage)
                );
        }

        /// <summary>
        /// It's a positive <see cref="int"/> (> 0).
        /// </summary>
        /// <param name="verify">the verify</param>
        /// <param name="value">value to be controlled</param>
        /// <param name="valueMessage">value message</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static void Positive(this IVerify verify, int value, [CallerArgumentExpression(nameof(value))] string? valueMessage = null)
        {
            if (verify is null)
            {
                throw new ArgumentNullException(nameof(verify));
            }
            verify.Satisfied(value > 0,
                assertValueMessage => new ArgumentOutOfRangeException(valueMessage,
                verify.NotOperatorEnabled ?
                    $"Parameter should be not positive. Condition [!({assertValueMessage})] not satisfied" :
                    $"Parameter should be positive. Condition [{assertValueMessage}] not satisfied")
                );
        }

        /// <summary>
        /// It's a negative <see cref="int"/> (< 0).
        /// </summary>
        /// <param name="verify">the verify</param>
        /// <param name="value">value to be controlled</param>
        /// <param name="valueMessage">value message</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static void Negative(this IVerify verify, int value, [CallerArgumentExpression(nameof(value))] string? valueMessage = null)
        {
            if (verify is null)
            {
                throw new ArgumentNullException(nameof(verify));
            }
            verify.Satisfied(value < 0,
                assertValueMessage => new ArgumentOutOfRangeException(valueMessage,
                verify.NotOperatorEnabled ?
                    $"Parameter should be not negative. Condition [!({assertValueMessage})] not satisfied" :
                    $"Parameter should be negative. Condition [{assertValueMessage}] not satisfied")
                );
        }
    }
}
