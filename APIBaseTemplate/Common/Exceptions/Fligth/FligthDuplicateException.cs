namespace APIBaseTemplate.Common.Exceptions
{
    public class FligthDuplicateException : FligthException
    {
        public FligthDuplicateException(
            string fieldName,
            object fieldValue
            ) : base(
                message: $"Field {fieldName} value already used",
                FligthErrorCodes.DUPLICATE_ERROR,
                (nameof(fieldName), fieldName, Visibility.Public),
                (nameof(fieldValue), fieldValue, Visibility.Private)
            )
        {

        }
    }
}
