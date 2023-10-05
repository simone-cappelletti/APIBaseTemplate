namespace APIBaseTemplate.Common.Exceptions
{
    public class AirportException : BaseException
    {
        public AirportException(
            string message,
            string errorCode = AirportErrorCodes.UNEXPECTED,
            params (string parameterName, object parameterValue, Visibility visibility)[] data) :
            base(message, errorCode, data)
        {

        }

        public AirportException(
            string message,
            Exception inner,
            string errorCode = AirportErrorCodes.UNEXPECTED,
            params (string parameterName, object parameterValue, Visibility visibility)[] data) :
            base(message, inner, errorCode, data)
        {

        }
    }
}
