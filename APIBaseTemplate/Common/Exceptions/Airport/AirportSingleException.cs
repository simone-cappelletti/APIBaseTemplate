namespace APIBaseTemplate.Common.Exceptions
{
    public class AirportSingleException : AirportException
    {
        public AirportSingleException(int airportId) :
            base($"Airport single fault", AirportErrorCodes.SINGLE_ERROR, (nameof(airportId), airportId, Visibility.Private))
        {
            HttpStatus = System.Net.HttpStatusCode.NotFound;
        }

        public AirportSingleException(int airportId, Exception inner) :
            base($"Airport single fault", inner, AirportErrorCodes.SINGLE_ERROR, (nameof(airportId), airportId, Visibility.Private))
        {
            HttpStatus = System.Net.HttpStatusCode.NotFound;
        }
    }
}
