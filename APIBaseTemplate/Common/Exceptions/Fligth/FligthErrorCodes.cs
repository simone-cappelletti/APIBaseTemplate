namespace APIBaseTemplate.Common.Exceptions
{
    public class FligthErrorCodes
    {
        /// <summary>
        /// A generic, unexpected error
        /// </summary>
        public const string UNEXPECTED = "flightErrorUnexpected";

        /// <summary>
        /// It was expected one and only one item
        /// </summary>
        public const string SINGLE_ERROR = "flightErrorSingle";
        public const string DUPLICATE_ERROR = "flightErrorDuplicate";

        /// <summary>
        /// Error while deleting
        /// </summary>
        public const string DELETE_ERROR = "flightErrorDelete";
    }
}
