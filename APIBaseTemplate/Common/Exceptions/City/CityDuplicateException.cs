namespace APIBaseTemplate.Common.Exceptions
{
    public class CityDuplicateException : CityException
    {
        public CityDuplicateException(
            string fieldName,
            object fieldValue
            ) : base(
                message: $"Field {fieldName} value already used",
                CityErrorCodes.DUPLICATE_ERROR,
                (nameof(fieldName), fieldName, Visibility.Public),
                (nameof(fieldValue), fieldValue, Visibility.Private)
            )
        {

        }
    }
}
