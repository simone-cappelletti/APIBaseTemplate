namespace APIBaseTemplate.Common.Exceptions
{
    public class FligthServiceErrorCodes
    {
        /// <summary>
        /// A generic, unexpected error
        /// </summary>
        public const string UNEXPECTED = "fligthServiceErrorUnexpected";

        /// <summary>
        /// It was expected one and only one item
        /// </summary>
        public const string SINGLE_ERROR = "fligthServiceErrorSingle";
        public const string DUPLICATE_ERROR = "fligthServiceErrorDuplicate";

        /// <summary>
        /// Error while deleting
        /// </summary>
        public const string DELETE_ERROR = "fligthServiceErrorDelete";
    }
}
