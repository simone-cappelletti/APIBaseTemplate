namespace APIBaseTemplate.Common.Exceptions
{
    public class AirportDuplicateException : AirportException
    {
        public AirportDuplicateException(
            string fieldName,
            object fieldValue
            ) : base(
                message: $"Field {fieldName} value already used",
                AirportErrorCodes.DUPLICATE_ERROR,
                (nameof(fieldName), fieldName, Visibility.Public),
                (nameof(fieldValue), fieldValue, Visibility.Private)
            )
        {

        }
    }
}
