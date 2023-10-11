namespace APIBaseTemplate.Common.Exceptions
{
    public class AirportErrorCodes
    {
        /// <summary>
        /// A generic, unexpected error
        /// </summary>
        public const string UNEXPECTED = "airportErrorUnexpected";

        /// <summary>
        /// It was expected one and only one item
        /// </summary>
        public const string SINGLE_ERROR = "airportErrorSingle";
        public const string DUPLICATE_ERROR = "airportErrorDuplicate";

        /// <summary>
        /// Error while deleting
        /// </summary>
        public const string DELETE_ERROR = "airportErrorDelete";
    }
}
