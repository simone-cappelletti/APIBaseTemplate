namespace APIBaseTemplate.Common.Exceptions
{
    public class CitySingleException : CityException
    {
        public CitySingleException(int cityId) :
            base($"City single fault", CityErrorCodes.SINGLE_ERROR, (nameof(cityId), cityId, Visibility.Private))
        {
            HttpStatus = System.Net.HttpStatusCode.NotFound;
        }

        public CitySingleException(int cityId, Exception inner) :
            base($"City single fault", inner, CityErrorCodes.SINGLE_ERROR, (nameof(cityId), cityId, Visibility.Private))
        {
            HttpStatus = System.Net.HttpStatusCode.NotFound;
        }
    }
}
