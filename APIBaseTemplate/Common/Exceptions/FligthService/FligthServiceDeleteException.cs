namespace APIBaseTemplate.Common.Exceptions
{
    public class FligthServiceDeleteException : FligthServiceException
    {
        public FligthServiceDeleteException(int fligthServiceId) :
            base("FligthService delete fault", FligthServiceErrorCodes.DELETE_ERROR, (nameof(fligthServiceId), fligthServiceId, Visibility.Private))
        {

        }

        public FligthServiceDeleteException(int fligthServiceId, Exception inner) :
            base("FligthService delete fault", inner, FligthServiceErrorCodes.DELETE_ERROR, (nameof(fligthServiceId), fligthServiceId, Visibility.Private))
        {

        }
    }
}
