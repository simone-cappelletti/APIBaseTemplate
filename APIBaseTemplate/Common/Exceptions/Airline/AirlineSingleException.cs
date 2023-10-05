namespace APIBaseTemplate.Common.Exceptions
{
    public class AirlineSingleException : AirlineException
    {
        public AirlineSingleException(int airlineId) :
            base($"Region single fault", AirlineErrorCodes.SINGLE_ERROR, (nameof(airlineId), airlineId, Visibility.Private))
        {
            HttpStatus = System.Net.HttpStatusCode.NotFound;
        }

        public AirlineSingleException(int airlineId, Exception inner) :
            base($"Region single fault", inner, AirlineErrorCodes.SINGLE_ERROR, (nameof(airlineId), airlineId, Visibility.Private))
        {
            HttpStatus = System.Net.HttpStatusCode.NotFound;
        }
    }
}
