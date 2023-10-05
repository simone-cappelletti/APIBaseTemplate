namespace APIBaseTemplate.Common.Exceptions
{
    public class RegionErrorCodes
    {
        /// <summary>
        /// A generic, unexpected error
        /// </summary>
        public const string UNEXPECTED = "regionErrorUnexpected";

        /// <summary>
        /// It was expected one and only one item
        /// </summary>
        public const string SINGLE_ERROR = "regionErrorSingle";
        public const string DUPLICATE_ERROR = "regionErrorDuplicate";

        /// <summary>
        /// Error while deleting
        /// </summary>
        public const string DELETE_ERROR = "regionErrorDelete";
    }
}
