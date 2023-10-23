namespace APIBaseTemplate.Common.Exceptions
{
    public class FligthServiceException : BaseException
    {
        public FligthServiceException(
            string message,
            string errorCode = FligthServiceErrorCodes.UNEXPECTED,
            params (string parameterName, object parameterValue, Visibility visibility)[] data) :
            base(message, errorCode, data)
        {

        }

        public FligthServiceException(
            string message,
            Exception inner,
            string errorCode = FligthServiceErrorCodes.UNEXPECTED,
            params (string parameterName, object parameterValue, Visibility visibility)[] data) :
            base(message, inner, errorCode, data)
        {

        }
    }
}
