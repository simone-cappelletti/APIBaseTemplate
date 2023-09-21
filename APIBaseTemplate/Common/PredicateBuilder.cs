using System.Linq.Expressions;

namespace APIBaseTemplate.Common
{
    /// <summary>
    /// Predicate builder by Joe Albahary
    /// https://social.msdn.microsoft.com/Forums/en-US/925b245d-5529-4a64-8cd4-4bc83ee6fe7a/dynamic-conditions-how-to-achieve-multiple-quotorquot-conditions-with-linq
    /// </summary>
    public static class PredicateBuilder
    {
        /// <summary>
        /// Placeholder Function to concatenate expressions in AND or OR
        /// </summary>
        /// <remarks>Always returns null</remarks>
        /// <typeparam name="T">the entity type</typeparam>
        /// <returns>null, but assignable  to an expression variable </returns>
        public static Expression<Func<T, bool>>? Make<T>() { return null; }

        /// <summary>
        /// Returns ther predicate provided as parameter.
        /// Useful to start AND/OR concatenation
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> Make<T>(this Expression<Func<T, bool>> predicate)
        {
            return predicate;
        }

        /// <summary>
        /// Returns a negated version of the predicate <see cref="Func{T, Boolean}"/>
        /// </summary>
        /// <typeparam name="T">Entity data</typeparam>
        /// <param name="predicate">the predicate labda</param>
        /// <returns> !predicate(TValue) </returns>
        public static Func<T, bool> Not<T>(this Func<T, bool> predicate)
        {
            return value => !predicate(value);
        }

        /// <summary>
        /// Returns a negated version of the Expression <see cref="Expression{Func{T, Boolean}}"/>
        /// </summary>
        /// <remarks>the thethod operates on <see cref="Expression"/> so it creates a new expression tree</remarks>
        /// <typeparam name="T">Entity data</typeparam>
        /// <param name="expr">the expression lambda</param>
        /// <returns> !expr(TValue) </returns>
        public static Expression<Func<T, bool>> Not<T>(
            this Expression<Func<T, bool>> expr)
        {
            return Expression.Lambda<Func<T, bool>>(
                Expression.Not(expr.Body),
                expr.Parameters);
        }

        /// <summary>
        /// OR between 2 expressions.
        /// Can be used to concatenate multiple expressions
        /// </summary>
        /// <typeparam name="T">the entity</typeparam>
        /// <param name="expr1">first parameter (if null the second will be returned)</param>
        /// <param name="expr2">second parameter</param>
        /// <returns><see cref="expr1"/> OR <see cref="expr2"/>. if <see cref="expr1"/> is null returns <see cref="expr2"/></returns>
        public static Expression<Func<T, bool>> OrElse<T>(
            this Expression<Func<T, bool>>? expr1, Expression<Func<T, bool>> expr2)
        {
            if (expr1 == null) return expr2;
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());

            return Expression.Lambda<Func<T, bool>>(
                Expression.OrElse(expr1.Body, invokedExpr),
                expr1.Parameters);
        }

        /// <summary>
        /// AND between 2 expressions.
        /// Can be used to concatenate multiple expressions
        /// </summary>
        /// <typeparam name="T">the entity</typeparam>
        /// <param name="expr1">first parameter (if null the second will be returned)</param>
        /// <param name="expr2">second parameter</param>
        /// <returns><see cref="expr1"/> AND <see cref="expr2"/>. if <see cref="expr1"/> is null returns <see cref="expr2"/></returns>
        public static Expression<Func<T, bool>> AndAlso<T>(
            this Expression<Func<T, bool>>? expr1, Expression<Func<T, bool>> expr2)
        {
            if (expr1 == null) return expr2;
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());

            return Expression.Lambda<Func<T, bool>>(
                Expression.AndAlso(expr1.Body, invokedExpr),
                expr1.Parameters);
        }
    }
}
