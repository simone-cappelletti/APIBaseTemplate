namespace APIBaseTemplate.Common.Exceptions
{
    public class RegionSingleException : RegionException
    {
        public RegionSingleException(int regionId) :
            base($"Region single fault", RegionErrorCodes.SINGLE_ERROR, (nameof(regionId), regionId, Visibility.Private))
        {
            HttpStatus = System.Net.HttpStatusCode.NotFound;
        }

        public RegionSingleException(int regionId, Exception inner) :
            base($"Region single fault", inner, RegionErrorCodes.SINGLE_ERROR, (nameof(regionId), regionId, Visibility.Private))
        {
            HttpStatus = System.Net.HttpStatusCode.NotFound;
        }
    }
}
