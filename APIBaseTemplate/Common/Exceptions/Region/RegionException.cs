namespace APIBaseTemplate.Common.Exceptions
{
    public class RegionException : BaseException
    {
        public RegionException(
            string message,
            string errorCode = RegionErrorCodes.UNEXPECTED,
            params (string parameterName, object parameterValue, Visibility visibility)[] data) :
            base(message, errorCode, data)
        {

        }

        public RegionException(
            string message,
            Exception inner,
            string errorCode = RegionErrorCodes.UNEXPECTED,
            params (string parameterName, object parameterValue, Visibility visibility)[] data) :
            base(message, inner, errorCode, data)
        {

        }
    }
}
