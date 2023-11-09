namespace APIBaseTemplate.Common.Exceptions
{
    public class RegionDuplicateException : RegionException
    {
        public RegionDuplicateException(
            string fieldName,
            object fieldValue
            ) : base(
                $"Field {fieldName} value already used",
                RegionErrorCodes.DUPLICATE_ERROR,
                (nameof(fieldName), fieldName, Visibility.Public),
                (nameof(fieldValue), fieldValue, Visibility.Private)
            )
        {

        }
    }
}
