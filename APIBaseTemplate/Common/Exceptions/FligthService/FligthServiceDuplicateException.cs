namespace APIBaseTemplate.Common.Exceptions
{
    public class FligthServiceDuplicateException : FligthServiceException
    {
        public FligthServiceDuplicateException(
            string fieldName,
            object fieldValue
            ) : base(
                message: $"Field {fieldName} value already used",
                FligthServiceErrorCodes.DUPLICATE_ERROR,
                (nameof(fieldName), fieldName, Visibility.Public),
                (nameof(fieldValue), fieldValue, Visibility.Private)
            )
        {

        }
    }
}
