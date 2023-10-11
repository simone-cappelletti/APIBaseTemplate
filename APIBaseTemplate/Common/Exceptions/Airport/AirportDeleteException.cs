namespace APIBaseTemplate.Common.Exceptions.Airport
{
    public class AirportDeleteException : AirportException
    {
        public AirportDeleteException(int airportId) :
            base("Airport delete fault", AirportErrorCodes.DELETE_ERROR, (nameof(airportId), airportId, Visibility.Private))
        {

        }

        public AirportDeleteException(int airportId, Exception inner) :
            base("Airport delete fault", inner, AirportErrorCodes.DELETE_ERROR, (nameof(airportId), airportId, Visibility.Private))
        {

        }
    }
}
