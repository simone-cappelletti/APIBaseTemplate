namespace APIBaseTemplate.Common.Exceptions
{
    public class CityDeleteException : CityException
    {
        public CityDeleteException(int cityId) :
            base("City delete fault", CityErrorCodes.DELETE_ERROR, (nameof(cityId), cityId, Visibility.Private))
        {

        }

        public CityDeleteException(int cityId, Exception inner) :
            base("City delete fault", inner, CityErrorCodes.DELETE_ERROR, (nameof(cityId), cityId, Visibility.Private))
        {

        }
    }
}
