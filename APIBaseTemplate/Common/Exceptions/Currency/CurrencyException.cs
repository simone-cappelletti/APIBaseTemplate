namespace APIBaseTemplate.Common.Exceptions
{
    public class CurrencyException : BaseException
    {
        public CurrencyException(
            string message,
            string errorCode = CurrencyErrorCodes.UNEXPECTED,
            params (string parameterName, object parameterValue, Visibility visibility)[] data) :
            base(message, errorCode, data)
        {

        }

        public CurrencyException(
            string message,
            Exception inner,
            string errorCode = CurrencyErrorCodes.UNEXPECTED,
            params (string parameterName, object parameterValue, Visibility visibility)[] data) :
            base(message, inner, errorCode, data)
        {

        }
    }
}
