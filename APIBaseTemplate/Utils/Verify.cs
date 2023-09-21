using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace APIBaseTemplate.Utils
{
    /// <summary>
    /// Verify class for validate objects
    /// </summary>
    public class Verify : IVerify
    {
        /// <inheritdoc/>
        public bool NotOperatorEnabled { get; init; }

        private Verify(bool notOperatorEnabled)
        {
            NotOperatorEnabled = notOperatorEnabled;
        }

        /// <summary>
        /// Is 
        /// </summary>
        public static IVerify Is
        {
            get
            {
                return new Verify(false);
            }
        }

        /// <summary>
        /// Is not  
        /// </summary>
        public static IVerify IsNot
        {
            get
            {
                var result = new Verify(true);
                return result;
            }
        }

        /// <inheritdoc/>
        public void Satisfied(
            bool condition,
            Func<string?, Exception>? exception = null,
            [CallerArgumentExpression(nameof(condition))] string? conditionArgument = null)
        {
            if (condition == NotOperatorEnabled)
            {
                exception ??= NotOperatorEnabled ?
                    m => new ArgumentException($"[!({m})] condition not satisfied") :
                    m => new ArgumentException($"[{m}] condition not satisfied");
                var ex = exception.Invoke(conditionArgument);
                throw ex;
            }
        }

        private void VerifyCondition(bool verified, string verifyName, string paramName)
        {
            string message = NotOperatorEnabled ?
                $"Verify {paramName} is not {verifyName} failed" :
                $"Verify {paramName} is {verifyName} failed";
            Satisfied(verified, _ => new ArgumentException(message));
        }

        /// <inheritdoc/>
        public void Null(object? obj, [CallerArgumentExpression(nameof(obj))] string? paramName = "")
        {
            Satisfied(obj == null, _ => new ArgumentNullException(paramName));
        }

        /// <inheritdoc/>
        public void Null(object? obj, Func<Exception> exception)
        {
            Satisfied(obj == null, _ => exception());
        }

        /// <inheritdoc/>
        public void NullOrEmpty(string? obj, string paramName = "")
        {
            VerifyCondition(string.IsNullOrEmpty(obj), "NullOrEmpty", paramName);
        }

        /// <inheritdoc/>
        public void NullOrEmpty(string? obj, Func<Exception> exception)
        {
            Satisfied(string.IsNullOrEmpty(obj), _ => exception());
        }

        /// <inheritdoc/>
        public void NullOrEmpty(Guid? obj, string paramName = "")
        {
            VerifyCondition(
                !obj.HasValue ||
                (obj.HasValue && obj.Value == Guid.Empty), $"NullOrEmpty", paramName);
        }

        /// <inheritdoc/>
        public void NullOrEmpty(Guid? obj, Func<Exception> exception)
        {
            Satisfied(
                !obj.HasValue ||
                (obj.HasValue && obj.Value == Guid.Empty), _ => exception());
        }

        /// <inheritdoc/>
        public void Empty(Guid obj, string paramName = "")
        {
            VerifyCondition(obj == Guid.Empty, $"Empty({paramName})", paramName);
        }

        /// <inheritdoc/>
        public void Empty(Guid obj, Func<Exception> exception)
        {
            Satisfied(obj == Guid.Empty, _ => exception());
        }

        /// <summary>
        /// Verify indicated condition
        /// </summary>
        /// <param name="condition">condition to be verified</param>
        /// <param name="exception">lambda for the exception</param>
        /// <param name="conditionArgument">condition argument</param>
        /// <exception cref="InvalidOperationException">Eccezione di default</exception>
        public static void That(
            Func<bool> condition,
            Func<Exception>? exception = null,
            [CallerArgumentExpression(nameof(condition))] string? conditionArgument = null)
        {
            var v = new Verify(false);
            var result = condition.Invoke();
            var onExceptionLambda = exception ?? (() => new ArgumentException($"Condition [{conditionArgument}] not verified"));
            v.Satisfied(result, _ => onExceptionLambda());
        }

        ///<inheritdoc/>
        public void StringAllowedMaximumLength(
            int maximumLength,
            string? obj,
            string paramName = "")
        {
            if (maximumLength < 0)
                throw new ArgumentException("Invalid maximum length", nameof(maximumLength));

            VerifyCondition(
                string.IsNullOrEmpty(obj) ||
                (
                    !string.IsNullOrEmpty(obj) &&
                    obj.Length <= maximumLength
                ),
                $"StringAllowedMaximumLength", paramName);
        }

        ///<inheritdoc/>
        public void StringAllowedMaximumLength(
            int maximumLength,
            string? obj,
            Func<Exception> exception)
        {
            Satisfied(
                string.IsNullOrEmpty(obj) ||
                (
                    !string.IsNullOrEmpty(obj) &&
                    obj.Length <= maximumLength
                ),
                _ => exception());
        }
    }
}
