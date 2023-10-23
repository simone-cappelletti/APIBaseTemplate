namespace APIBaseTemplate.Common.Exceptions
{
    public class FligthServiceSingleException : FligthServiceException
    {
        public FligthServiceSingleException(int fligthServiceId) :
            base($"FligthService single fault", FligthServiceErrorCodes.SINGLE_ERROR, (nameof(fligthServiceId), fligthServiceId, Visibility.Private))
        {
            HttpStatus = System.Net.HttpStatusCode.NotFound;
        }

        public FligthServiceSingleException(int fligthServiceId, Exception inner) :
            base($"FligthService single fault", inner, FligthServiceErrorCodes.SINGLE_ERROR, (nameof(fligthServiceId), fligthServiceId, Visibility.Private))
        {
            HttpStatus = System.Net.HttpStatusCode.NotFound;
        }
    }
}
