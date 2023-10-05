namespace APIBaseTemplate.Common.Exceptions
{
    public class RegionDeleteException : RegionException
    {
        public RegionDeleteException(int regionId) :
            base("Region delete fault", RegionErrorCodes.DELETE_ERROR, (nameof(regionId), regionId, Visibility.Private))
        {

        }

        public RegionDeleteException(int regionId, Exception inner) :
            base("Region delete fault", inner, RegionErrorCodes.DELETE_ERROR, (nameof(regionId), regionId, Visibility.Private))
        {

        }
    }
}
