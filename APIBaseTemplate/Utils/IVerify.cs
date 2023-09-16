using System.Runtime.CompilerServices;

namespace APIBaseTemplate.Utils
{
    /// <summary>
    /// An interface to verify expected status of items (object, string, guid,...)
    /// </summary>
    public interface IVerify
    {
        /// <summary>
        /// Whether the negation operator has been invoked <see cref="Verify.IsNot"/>
        /// </summary>
        bool NotOperatorEnabled { get; }

        /// <summary>
        /// Verify (assert) that the indicated boolean condition is verified. <br />
        /// - Case <see cref="NotOperatorEnabled"/> = false (<see cref="Verify.Is"/>) => <see cref="verified"/> must be <see cref="true"/>  <br />
        /// - Case <see cref="NotOperatorEnabled"/> = true (<see cref="Verify.IsNot"/>) => <see cref="verified"/> must be<see cref="false"/>  <br />
        /// </summary>
        /// <param name="condition">condition result that must be satisfied</param>
        /// <param name="exception"> if empty an <see cref="ArgumentException"/> will be raised</param>
        /// <param name="conditionMessage">expression indicated in <see cref="verified"/></param>
        /// <exception cref="InvalidOperationException">
        /// Default exception raised if lambda exceptionis not specified
        /// </exception>
        void Satisfied(bool condition, Func<string?, Exception>? exception = null, [CallerArgumentExpression(nameof(condition))] string? conditionMessage = null);

        /// <summary>
        /// Guid is empty
        /// </summary>
        /// <param name="obj">parameter to verify</param>
        /// <param name="paramName">parameter name</param>
        /// <exception cref="ArgumentNullException"></exception>
        void Empty(Guid obj, string paramName = "");

        /// <summary>
        /// Guid is empty
        /// </summary>
        /// <param name="obj">parameter to verify</param>
        /// <param name="exception">parameter name</param>
        /// <exception cref="Exception"></exception>
        void Empty(Guid obj, Func<Exception> exception);

        /// <summary>
        /// It's null
        /// </summary>
        /// <param name="obj">parameter to verify</param>
        /// <param name="paramName">parameter name</param>
        /// <exception cref="ArgumentNullException"></exception>
        void Null(object? obj, string paramName = "");

        /// <summary>
        /// It's null
        /// </summary>
        /// <param name="obj">parameter to verify</param>
        /// <param name="exception">lambda for the exception</param>
        /// <exception cref="Exception"></exception>
        void Null(object? obj, Func<Exception> exception);

        /// <summary>
        /// Guid is empty or null
        /// </summary>
        /// <param name="obj">parameter to verify</param>
        /// <param name="paramName">parameter name</param>
        /// <exception cref="ArgumentNullException"></exception>
        void NullOrEmpty(Guid? obj, string paramName = "");

        /// <summary>
        /// Guid is empty or null
        /// </summary>
        /// <param name="obj">parameter to verify</param>
        /// <param name="exception">lambda for the exception</param>
        /// <exception cref="Exception"></exception>
        void NullOrEmpty(Guid? obj, Func<Exception> exception);

        /// <summary>
        /// String is empty or null
        /// </summary>
        /// <param name="obj">parameter to verify</param>
        /// <param name="paramName">parameter name</param>
        /// <exception cref="ArgumentNullException"></exception>
        void NullOrEmpty(string? obj, string paramName = "");

        /// <summary>
        /// String is empty or null
        /// </summary>
        /// <param name="obj">parameter to verify</param>
        /// <param name="exception">lambda for the exception</param>
        /// <exception cref="Exception"></exception>
        void NullOrEmpty(string? obj, Func<Exception> exception);

        /// <summary>
        /// String has a maximun length
        /// </summary>
        /// <param name="maximumLength">maximum length</param>
        /// <param name="obj">parameter to verify</param>
        /// <param name="paramName">parameter name</param>
        void StringAllowedMaximumLength(int maximumLength, string? obj, string paramName = "");

        /// <summary>
        /// String has a maximun length
        /// </summary>
        /// <param name="maximumLength">maximum length</param>
        /// <param name="obj">parameter to verify</param>
        /// <param name="exception">lambda for the exception</param>
        void StringAllowedMaximumLength(int maximumLength, string? obj, Func<Exception> exception);
    }
}
