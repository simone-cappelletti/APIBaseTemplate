namespace APIBaseTemplate.Common.Exceptions
{
    public class AirlineDuplicateException : AirlineException
    {
        public AirlineDuplicateException(
            string fieldName,
            object fieldValue
            ) : base(
                message: $"Field {fieldName} value already used",
                AirlineErrorCodes.DUPLICATE_ERROR,
                (nameof(fieldName), fieldName, Visibility.Public),
                (nameof(fieldValue), fieldValue, Visibility.Private)
            )
        {

        }
    }
}
