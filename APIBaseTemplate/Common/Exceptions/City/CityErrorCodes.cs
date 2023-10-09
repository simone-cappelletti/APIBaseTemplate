namespace APIBaseTemplate.Common.Exceptions
{
    public class CityErrorCodes
    {
        /// <summary>
        /// A generic, unexpected error
        /// </summary>
        public const string UNEXPECTED = "cityErrorUnexpected";

        /// <summary>
        /// It was expected one and only one item
        /// </summary>
        public const string SINGLE_ERROR = "cityErrorSingle";
        public const string DUPLICATE_ERROR = "cityErrorDuplicate";

        /// <summary>
        /// Error while deleting
        /// </summary>
        public const string DELETE_ERROR = "cityErrorDelete";
    }
}
