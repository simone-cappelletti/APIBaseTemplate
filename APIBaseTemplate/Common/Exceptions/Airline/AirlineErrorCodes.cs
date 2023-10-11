namespace APIBaseTemplate.Common.Exceptions
{
    public class AirlineErrorCodes
    {
        /// <summary>
        /// A generic, unexpected error
        /// </summary>
        public const string UNEXPECTED = "airlineErrorUnexpected";

        /// <summary>
        /// It was expected one and only one item
        /// </summary>
        public const string SINGLE_ERROR = "airlineErrorSingle";
        public const string DUPLICATE_ERROR = "airlineErrorDuplicate";

        /// <summary>
        /// Error while deleting
        /// </summary>
        public const string DELETE_ERROR = "airlineErrorDelete";
    }
}
