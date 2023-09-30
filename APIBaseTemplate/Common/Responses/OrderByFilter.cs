using System.Linq.Expressions;
using System.Reflection;

namespace APIBaseTemplate.Common
{
    /// <summary>
    /// Class to simplify sorting columns in grids..
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    public class OrderByFilter<TSource>
    {
        private readonly Type _sourceType;

        private readonly Dictionary<string, (object, OrderByMethods)> _criteria;
        private readonly Dictionary<Type, OrderByMethods> _orderByMethods;

        private MethodInfo? _orderByMethodDefinition;
        private MethodInfo? _orderByDescendingMethodDefinition;
        private MethodInfo? _thenByMethodDefinition;
        private MethodInfo? _thenByDescendingMethodDefinition;

        /// <summary>
        /// Create a new empty sorting criteria
        /// </summary>
        public OrderByFilter()
        {
            _sourceType = typeof(TSource);
            _criteria = new Dictionary<string, (object, OrderByMethods)>();
            _orderByMethods = new Dictionary<Type, OrderByMethods>();

            LoadIQueryableMethods();
        }

        public string GetAvailableFieldsForSorting()
        {
            if (_criteria != null && _criteria.Any())
            {
                return string.Join(',', _criteria.Keys.OrderBy(i => i));
            }
            else
            {
                return string.Empty;
            }
        }

        public string GetFirstAvailbleFieldForSorting()
        {
            if (_criteria != null && _criteria.Any())
            {
                return _criteria.Keys.First();
            }
            else
            {
                return string.Empty;
            }
        }

        public string GetSourceTypeName() => _sourceType.Name;

        /// <summary>
        /// Initialize <see cref="_orderByMethodDefinition"/>,
        /// <see cref="_orderByDescendingMethodDefinition"/>,
        /// <see cref="_thenByMethodDefinition"/>,
        /// <see cref="_thenByDescendingMethodDefinition"/>
        /// </summary>
        public void LoadIQueryableMethods()
        {
            Type queryable = typeof(System.Linq.Queryable);
            var methods = queryable.GetMethods();

            // public static IOrderedQueryable<TSource> OrderBy<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector);
            _orderByMethodDefinition = methods.Single(x =>
                x.IsGenericMethod &&
                x.IsGenericMethodDefinition &&
                x.Name == nameof(System.Linq.Queryable.OrderBy) &&
                x.GetParameters().Length == 2);

            // OrderByDescending
            _orderByDescendingMethodDefinition = methods.Single(x =>
                x.IsGenericMethod &&
                x.IsGenericMethodDefinition &&
                x.Name == nameof(System.Linq.Queryable.OrderByDescending) &&
                x.GetParameters().Length == 2);

            // public static IOrderedQueryable<TSource> ThenBy<TSource, TKey>(this IOrderedQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector);
            _thenByMethodDefinition = methods.Single(x =>
                x.IsGenericMethod &&
                x.IsGenericMethodDefinition &&
                x.Name == nameof(Queryable.ThenBy) &&
                x.GetParameters().Length == 2);

            // ThenByDescending
            _thenByDescendingMethodDefinition = methods.Single(x =>
                x.IsGenericMethod &&
                x.IsGenericMethodDefinition &&
                x.Name == nameof(System.Linq.Queryable.ThenByDescending) &&
                x.GetParameters().Length == 2);
        }

        /// <summary>
        /// Return list of all supported search criteria
        /// </summary>
        /// <returns></returns>
        public string[] GetOrderByKeys() => _criteria.Keys.ToArray();

        /// <summary>
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="orderByKey"></param>
        /// <param name="keySelector"></param>
        /// <returns>this, per concatenare più chiamate</returns>
        public OrderByFilter<TSource> Add<TKey>(string orderByKey, Expression<Func<TSource, TKey>> keySelector)
        {
            var keyType = typeof(TKey);
            if (!_orderByMethods.ContainsKey(keyType))
            {
                _orderByMethods[keyType] = MakeOrderByMethod(keyType);
            }

            var orderBy = _orderByMethods[keyType];

            _criteria.Add(orderByKey, (keySelector, orderBy));

            return this;
        }

        private OrderByMethods MakeOrderByMethod(Type keyType)
        {
            if (_orderByMethodDefinition == null)
            {
                throw new InvalidOperationException($"{nameof(_orderByMethodDefinition)} is null, unable to {nameof(MakeOrderByMethod)}");
            }

            if (_orderByDescendingMethodDefinition == null)
            {
                throw new InvalidOperationException($"{nameof(_orderByDescendingMethodDefinition)} is null, unable to {nameof(MakeOrderByMethod)}");
            }

            if (_thenByMethodDefinition == null)
            {
                throw new InvalidOperationException($"{nameof(_thenByMethodDefinition)} is null, unable to {nameof(MakeOrderByMethod)}");
            }

            if (_thenByDescendingMethodDefinition == null)
            {
                throw new InvalidOperationException($"{nameof(_thenByDescendingMethodDefinition)} is null, unable to {nameof(MakeOrderByMethod)}");
            }


            MethodInfo orderBy = _orderByMethodDefinition.MakeGenericMethod(_sourceType, keyType);
            MethodInfo orderByDescending = _orderByDescendingMethodDefinition.MakeGenericMethod(_sourceType, keyType); ;
            MethodInfo thenBy = _thenByMethodDefinition.MakeGenericMethod(_sourceType, keyType);
            MethodInfo thenByDescending = _thenByDescendingMethodDefinition.MakeGenericMethod(_sourceType, keyType);

            var result = new OrderByMethods(orderBy, orderByDescending, thenBy, thenByDescending);

            return result;
        }

        /// <summary>
        /// Apply to the query a query the list of orderby indicated in the oderBy parameter.
        /// If the list is empty, apply a default sort order.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="orderBy"></param>
        /// <param name="defaultOrderBy"></param>
        /// <returns></returns>
        public IOrderedQueryable<TSource> OrderBy(IQueryable<TSource> query, IEnumerable<OrderByOption> orderBy, Func<IQueryable<TSource>, IOrderedQueryable<TSource>> defaultOrderBy)
        {
            orderBy = orderBy ?? OrderByOption.EmptyOptionList;

            if (orderBy.Any())
            {
                // OrderBy
                var queryOrderBy = ApplyOrderBy(query, orderBy.First());

                if (orderBy.Count() > 1)
                {
                    var thenOrderBy = orderBy.Skip(1).ToArray();

                    foreach (var thenBy in thenOrderBy)
                    {
                        queryOrderBy = ApplyThenBy(queryOrderBy, thenBy);
                    }
                }

                return queryOrderBy;
            }
            else
            {
                return defaultOrderBy(query);
            }
        }

        private IOrderedQueryable<TSource> ApplyOrderBy(IQueryable<TSource> query, OrderByOption orderByOption)
        {
            if (!_criteria.ContainsKey(orderByOption.Field))
            {
                throw new InvalidOperationException($"Unsupported sorting field '{orderByOption}' for type {_sourceType.FullName}");
            }

            (object lambda, OrderByMethods methods) = _criteria[orderByOption.Field];
            MethodInfo func = orderByOption.Direction == EnmOrderBy.Asc ? methods.OrderBy : methods.OrderByDescending;

            object? res = func.Invoke(obj: null, new object[] { query, lambda });
            if (res == null)
            {
                throw new InvalidOperationException("Unable to apply order by (or order by descending) clause because Invoke result is null");
            }

            IOrderedQueryable<TSource> result = (IOrderedQueryable<TSource>)res;

            return result;
        }

        private IOrderedQueryable<TSource> ApplyThenBy(IOrderedQueryable<TSource> queryOrderBy, OrderByOption thenSortBy)
        {
            if (!_criteria.ContainsKey(thenSortBy.Field))
            {
                throw new InvalidOperationException($"Unsupported sorting field '{thenSortBy}' for type {_sourceType.FullName}");
            }

            (object lambda, OrderByMethods methods) = _criteria[thenSortBy.Field];
            MethodInfo func = thenSortBy.Direction == EnmOrderBy.Asc ? methods.ThenBy : methods.ThenByDescending;

            object? res = func.Invoke(obj: null, new object[] { queryOrderBy, lambda });
            if (res == null)
            {
                throw new InvalidOperationException("Unable to apply then by (or then by descending) clause because Invoke result is null");
            }

            IOrderedQueryable<TSource> result = (IOrderedQueryable<TSource>)res;

            return result;
        }

    }
}
