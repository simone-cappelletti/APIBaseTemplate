namespace APIBaseTemplate.Common.Exceptions
{
    public class FligthSingleException : FligthException
    {
        public FligthSingleException(int fligthId) :
            base($"Fligth single fault", FligthErrorCodes.SINGLE_ERROR, (nameof(fligthId), fligthId, Visibility.Private))
        {
            HttpStatus = System.Net.HttpStatusCode.NotFound;
        }

        public FligthSingleException(int fligthId, Exception inner) :
            base($"Fligth single fault", inner, FligthErrorCodes.SINGLE_ERROR, (nameof(fligthId), fligthId, Visibility.Private))
        {
            HttpStatus = System.Net.HttpStatusCode.NotFound;
        }
    }
}
