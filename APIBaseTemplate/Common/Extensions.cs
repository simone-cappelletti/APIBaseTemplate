using APIBaseTemplate.Datamodel;
using APIBaseTemplate.Repositories;
using APIBaseTemplate.Repositories.DataContexts;
using APIBaseTemplate.Repositories.UnitOfWork;
using APIBaseTemplate.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Linq.Expressions;
using System.Linq;
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

    public static class QueryableTextFilterExtensions
    {
        /// <summary>
        /// Apply the Text Filter if not null, using the lambda indicated
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="query">the query to filter</param>
        /// <param name="filterOperator"></param>
        /// <param name="isNull"> Where isNull predicate</param>
        /// <param name="equalTo">Where equalTo predicate</param>
        /// <param name="like">Where like predicate</param>
        /// <param name="startsWith">Where startsWith predicate</param>
        /// <param name="endsWith">Where endsWith predicate</param>
        /// <param name="lessThan">Where lessThan predicate</param>
        /// <param name="greaterThan">Where greaterThan predicate</param>
        /// <param name="inValues">Where valuesIn predicate</param>
        /// <returns></returns>
        public static IQueryable<T> WhereTextFilter<T>(this IQueryable<T> query, EnmTextFilterOperator filterOperator,
            Expression<Func<T, bool>>? isNull,
            Expression<Func<T, bool>> equalTo,
            Expression<Func<T, bool>> like,
            Expression<Func<T, bool>> startsWith,
            Expression<Func<T, bool>> endsWith,
            Expression<Func<T, bool>> lessThan,
            Expression<Func<T, bool>> greaterThan,
            Expression<Func<T, bool>>? inValues)
        {
            query = (filterOperator) switch
            {
                EnmTextFilterOperator.IsNull =>
                    isNull != null ?
                    query.Where(isNull) :
                    throw new ArgumentNullException(
                        nameof(isNull),
                        $"Operator {filterOperator} was expected"),

                EnmTextFilterOperator.IsNotNull =>
                    isNull != null ?
                    query.Where(isNull.Not()) :
                    throw new ArgumentNullException(
                        nameof(isNull),
                        $"Operator {filterOperator} was expected"),

                EnmTextFilterOperator.Contains =>
                    like != null ?
                    query.Where(like) :
                    throw new ArgumentNullException(
                        nameof(like),
                        $"Operator {filterOperator} was expected"),

                EnmTextFilterOperator.NotContains =>
                    like != null ?
                    query.Where(like.Not()) :
                    throw new ArgumentNullException(
                        nameof(like),
                        $"Operator {filterOperator} was expected"),

                EnmTextFilterOperator.StartsWith =>
                    startsWith != null ?
                    query.Where(startsWith) :
                    throw new ArgumentNullException(
                        nameof(startsWith),
                        $"Operator {filterOperator} was expected"),

                EnmTextFilterOperator.NotStartsWith =>
                    startsWith != null ?
                    query.Where(startsWith.Not()) :
                    throw new ArgumentNullException(
                        nameof(startsWith),
                        $"Operator {filterOperator} was expected"),

                EnmTextFilterOperator.EndsWith =>
                    endsWith != null ?
                    query.Where(endsWith) :
                    throw new ArgumentNullException(
                        nameof(endsWith),
                        $"Operator {filterOperator} was expected"),

                EnmTextFilterOperator.NotEndsWith =>
                    endsWith != null ?
                    query.Where(endsWith.Not()) :
                    throw new ArgumentNullException(
                        nameof(endsWith),
                        $"Operator {filterOperator} was expected"),

                EnmTextFilterOperator.EqualTo =>
                    equalTo != null ?
                    query.Where(equalTo) :
                    throw new ArgumentNullException(
                        nameof(equalTo),
                        $"Operator {filterOperator} was expected"),

                EnmTextFilterOperator.NotEqualTo =>
                    equalTo != null ?
                    query.Where(equalTo.Not()) :
                    throw new ArgumentNullException(
                        nameof(equalTo),
                        $"Operator {filterOperator} was expected"),

                EnmTextFilterOperator.LessThan =>
                    lessThan != null ?
                    query.Where(lessThan) :
                    throw new ArgumentNullException(
                        nameof(lessThan),
                        $"Operator {filterOperator} was expected"),

                // 'x <= 5' it's the same thing as '!(x > 5)'
                EnmTextFilterOperator.LessThanEqual =>
                    greaterThan != null ?
                    query.Where(greaterThan.Not()) :
                    throw new ArgumentNullException(
                        nameof(greaterThan),
                        $"Operator {greaterThan} was expected (lte operator use gt operator)"),

                EnmTextFilterOperator.GreaterThan =>
                    greaterThan != null ?
                    query.Where(greaterThan) :
                    throw new ArgumentNullException(
                        nameof(greaterThan),
                        $"Operator {filterOperator} was expected"),

                // 'x >= 5' it's the same thing as '!(x < 5)'
                EnmTextFilterOperator.GreaterThanEqual =>
                    lessThan != null ?
                    query.Where(lessThan.Not()) :
                    throw new ArgumentNullException(
                        nameof(lessThan),
                        $"Operator {lessThan} was expected (gte operator use lt operator)"),

                EnmTextFilterOperator.InValues =>
                    inValues != null ?
                    query.Where(inValues) :
                    throw new ArgumentNullException(
                        nameof(inValues),
                        $"Operator {filterOperator} was expected"),

                EnmTextFilterOperator.NotInValues =>
                    inValues != null ?
                    query.Where(inValues.Not()) :
                    throw new ArgumentNullException(
                        nameof(inValues),
                        $"Operator {filterOperator} was expected"),

                _ => throw new ArgumentOutOfRangeException(
                    $"Unsupported value {filterOperator}"), // valore non previsto
            };
            return query;
        }

    }
}
