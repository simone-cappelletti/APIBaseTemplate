namespace APIBaseTemplate.Common.Exceptions
{
    public class CityException : BaseException
    {
        public CityException(
            string message,
            string errorCode = CityErrorCodes.UNEXPECTED,
            params (string parameterName, object parameterValue, Visibility visibility)[] data) :
            base(message, errorCode, data)
        {

        }

        public CityException(
            string message,
            Exception inner,
            string errorCode = CityErrorCodes.UNEXPECTED,
            params (string parameterName, object parameterValue, Visibility visibility)[] data) :
            base(message, inner, errorCode, data)
        {

        }
    }
}
