namespace APIBaseTemplate.Common.Exceptions
{
    public class FligthException : BaseException
    {
        public FligthException(
            string message,
            string errorCode = FligthErrorCodes.UNEXPECTED,
            params (string parameterName, object parameterValue, Visibility visibility)[] data) :
            base(message, errorCode, data)
        {

        }

        public FligthException(
            string message,
            Exception inner,
            string errorCode = FligthErrorCodes.UNEXPECTED,
            params (string parameterName, object parameterValue, Visibility visibility)[] data) :
            base(message, inner, errorCode, data)
        {

        }
    }
}
