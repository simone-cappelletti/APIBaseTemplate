namespace APIBaseTemplate.Common.Exceptions
{
    public class AirlineException : BaseException
    {
        public AirlineException(
            string message,
            string errorCode = AirlineErrorCodes.UNEXPECTED,
            params (string parameterName, object parameterValue, Visibility visibility)[] data) :
            base(message, errorCode, data)
        {

        }

        public AirlineException(
            string message,
            Exception inner,
            string errorCode = AirlineErrorCodes.UNEXPECTED,
            params (string parameterName, object parameterValue, Visibility visibility)[] data) :
            base(message, inner, errorCode, data)
        {

        }
    }
}
