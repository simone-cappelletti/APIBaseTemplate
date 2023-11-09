using System.Net;

namespace APIBaseTemplate.Common
{
    /// <summary>
    /// A base exception class
    /// </summary>
    /// <remarks>
    /// Some thoughts:
    /// Use predefined exception when possible (e.g. <see cref="ArgumentException"/>, <see cref="ArgumentNullException"/>, ...);
    /// It's reccomended not to use <see cref="BaseException"/> but extending it, incorporating an error code specific for the application
    /// </remarks>
    [Serializable]
    public class BaseException : Exception
    {
        /// <summary>
        /// An error code which is also a translation key.
        /// </summary>
        /// <remarks>
        /// For values to use in interpolation look at values of dictionary <see cref="PublicErrorCodeParameters"/>
        /// </remarks>
        public virtual string ErrorCode { get; } = GenericErrorCodes.UNEXPECTED;

        /// <summary>
        /// Public error code parameters.
        /// These must not contain any sensitive data and can be sent to a remote client without any security issue.
        /// </summary>
        public Dictionary<string, object> PublicErrorCodeParameters { get; private set; } = new Dictionary<string, object>();

        /// <summary>
        /// Public and private error code parameters.
        /// These could contain some sensitive data and cannot be sent to a remote client.
        /// However this data could help to write some important diagnostic data in a 'secure' log.
        /// </summary>
        public Dictionary<string, object> PublicAndPrivateErrorCodeParameters { get; private set; } = new Dictionary<string, object>();

        /// <summary>
        /// Timestamp error
        /// </summary>
        public DateTime TimestampUtc { get; set; }

        /// <summary>
        /// Suggested http Status Code (400, 500, ...) for rest return.
        /// </summary>
        /// <remarks>
        /// Default value is <see cref="HttpStatusCode.InternalServerError"/>
        /// </remarks>
        public HttpStatusCode? HttpStatus { get; protected set; } = HttpStatusCode.InternalServerError;

        #region ctors

        /// <summary>
        /// Initialize a base exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="errorCode"></param>
        /// <param name="data">
        /// Additional parameters.
        /// Parameters which are public can be sent to external client.
        /// </param>
        /// <example>
        /// new BaseException($"Name {fligth.FligthCode} already used",
        ///                   Constants.GenericErrorCodes.ITEM_ALREADY_EXISTS,
        ///                   ("fligthCode", "fligthCode1", ParamVisibility.Public), 
        ///                   ("anotherParameterName", "parameter2", ParamVisibility.Public))
        /// </example>
        public BaseException(
            string message,
            string errorCode = GenericErrorCodes.UNEXPECTED,
            params (string parameterName, object? parameterValue, Visibility visibility)[] data)
            : base(message)
        {
            TimestampUtc = DateTime.UtcNow;
            ErrorCode = errorCode;

            if (data != null)
            {
                InitErrorCodeParameters(data);
            }
        }

        /// <summary>
        /// Initialize a base exception
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        /// <param name="errorCode"></param>
        /// <param name="data">
        /// Additional parameters.
        /// Parameters which aare public can be sended to external client
        /// </param>
        /// <example>
        /// new BaseException($"Name {company.CompanyName} already used",
        ///                   innerException,
        ///                   Constants.GenericErrorCodes.ITEM_ALREADY_EXISTS,
        ///                   ("companyName", "companyName1", ParamVisibility.PrivateAndPublic), ("anotherParameterName", "parameter2", ParamVisibility.Public))
        /// </example>
        public BaseException(string message,
            Exception inner,
            string errorCode = GenericErrorCodes.UNEXPECTED,
            params (string parameterName, object? parameterValue, Visibility visibility)[] data)
            : base(message, inner)
        {
            TimestampUtc = DateTime.UtcNow;
            ErrorCode = errorCode;

            if (data != null)
            {
                InitErrorCodeParameters(data);
            }
        }

        /// <summary>
        /// Initialize a base exception
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected BaseException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }

        #endregion ctors

        /// <summary>
        /// From 3-tuple <paramref name="errorParameters"/> initialize <see cref="PublicErrorCodeParameters"/> and <see cref="PublicAndPrivateErrorCodeParameters"/>
        /// </summary>
        /// <param name="errorParameters"></param>
        protected void InitErrorCodeParameters((string parameterName, object? parameterValue, Visibility visibility)[] errorParameters)
        {
            if (errorParameters == null) return;
            if (errorParameters.Length == 0) return;

            // dispatching errorParameters: I will end with the following collections:
            // - PublicErrorCodeParameters: will not contains private stuff but only public ones
            // - PublicAndPrivateErrorCodeParameters: will contains both private and public stuff
            foreach ((string parameterName, object? parameterValue, Visibility visibility) errorParameter in errorParameters)
            {
                if (errorParameter.visibility == Visibility.Private)
                {
                    // Visibility.Private error parameter:
                    // goes only in PublicAndPrivateErrorCodeParameters.
                    PublicAndPrivateErrorCodeParameters.Add(errorParameter.Item1, errorParameter.Item2 ?? "");
                }
                else
                {
                    // Visibility.Public error parameter:
                    // goes in both collections PublicAndPrivateErrorCodeParameters and PublicErrorCodeParameters.
                    PublicErrorCodeParameters.Add(errorParameter.Item1, errorParameter.Item2 ?? "");
                    PublicAndPrivateErrorCodeParameters.Add(errorParameter.Item1, errorParameter.Item2 ?? "");
                }
            }
        }
    }

    /// <summary>
    /// Visibility
    /// </summary>
    public enum Visibility
    {
        /// <summary>
        /// Should be maintained private (e.g. visibile only in server-side log, console output)
        /// </summary>
        Private = 0,

        /// <summary>
        /// Could be made public (e.g. sent back to client)
        /// </summary>
        Public = 1
    }
}
