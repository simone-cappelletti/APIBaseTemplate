namespace APIBaseTemplate.Common.Exceptions
{
    public class CurrencyErrorCodes
    {
        /// <summary>
        /// A generic, unexpected error
        /// </summary>
        public const string UNEXPECTED = "currencyErrorUnexpected";

        /// <summary>
        /// It was expected one and only one item
        /// </summary>
        public const string SINGLE_ERROR = "currencyErrorSingle";
        public const string DUPLICATE_ERROR = "currencyErrorDuplicate";

        /// <summary>
        /// Error while deleting
        /// </summary>
        public const string DELETE_ERROR = "currencyErrorDelete";
    }
}
