namespace APIBaseTemplate.Common.Exceptions
{
    public class FligthDeleteException : FligthException
    {
        public FligthDeleteException(int fligthId) :
            base("Fligth delete fault", FligthErrorCodes.DELETE_ERROR, (nameof(fligthId), fligthId, Visibility.Private))
        {

        }

        public FligthDeleteException(int fligthId, Exception inner) :
            base("Fligth delete fault", inner, FligthErrorCodes.DELETE_ERROR, (nameof(fligthId), fligthId, Visibility.Private))
        {

        }
    }
}
