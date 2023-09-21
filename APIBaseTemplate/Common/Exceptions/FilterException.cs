namespace APIBaseTemplate.Common
{
    public class FilterException : BaseException
    {
        public FilterException(
            string errorMessage,
            string errorCode,
            EnmFilterTypes filterType,
            object @operator,
            object? value = null,
            object? value2 = null) :
            base(errorMessage, errorCode,
                ("filterType", filterType, Visibility.Public),
                ("operator", @operator, Visibility.Public),
                ("value", value, Visibility.Public),
                ("value2", value2, Visibility.Public)
                )
        { }
    }
}
