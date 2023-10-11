namespace APIBaseTemplate.Common.Exceptions
{
    public class AirlineDeleteException : AirlineException
    {
        public AirlineDeleteException(int airlineId) :
            base("Airline delete fault", AirlineErrorCodes.DELETE_ERROR, (nameof(airlineId), airlineId, Visibility.Private))
        {

        }

        public AirlineDeleteException(int cityId, Exception inner) :
            base("Airline delete fault", inner, AirlineErrorCodes.DELETE_ERROR, (nameof(cityId), cityId, Visibility.Private))
        {

        }
    }
}
