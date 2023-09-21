namespace APIBaseTemplate.Common
{
    public class GenericErrorCodes
    {
        public string NO_ERROR = "noError";

        /// <summary>
        /// A generic, unexpected error
        /// </summary>
        public const string UNEXPECTED = "errorUnexpected";
        public const string UNEXPECTED_EXTRA_DATA = "errorUnexpectedExtraData";

        /// <summary>
        /// Invalid data provided (e.g. formally invalid or somehow inconsistent with each other)
        /// </summary>
        public string INVALID_INPUT_DATA = "errorInvalidInputData";
        public string EXCEED_FIELD_MAXLENGHT = "errorExceedFieldMaxLenght";
        public string OUT_OF_RANGE_ARGUMENT = "errorArgumentOutOfRange";

        /// <summary>
        /// Necessary data not present (or no data present)
        /// </summary>
        public string MISSING_REQUIRED_INPUT_DATA = "errorMissingRequiredInputData";

        /// <summary>
        /// The executed operation is invalid
        /// </summary>
        public string INVALID_OPERATION = "errorInvalidOperation";

        /// <summary>
        /// Problem encountered when communicating with db
        /// </summary>
        /// <example>
        /// Broken db connection
        /// </example>
        public string DB_COMMUNICATION = "errorDbCommunication";

        /// <summary>
        /// Missing authentication
        /// </summary>
        public string NOT_AUTHENTICATED = "errorNotAuthenticated";

        /// <summary>
        /// Missing permission
        /// </summary>
        public string NOT_AUTHORIZED = "errorNotAuthorized";

        /// <summary>
        /// Data expected but not found.
        /// </summary>
        /// <example>
        /// A specific record was requested by identification, but that record was not found.
        /// </example>
        /// <remarks>
        /// This error code is not to be used when a search does not produce results.
        /// </remarks>
        public const string DATA_NOT_FOUND = "errorDataNotFound";

        /// <summary>
        /// Existing element
        /// </summary>
        /// <example>
        /// You are trying to insert an item with the same name or updating an existing item, the new name already exists.
        /// </example>
        public string ITEM_ALREADY_EXISTS = "errorItemAlreadyExists";

        /// <summary>
        /// It was expected one and only one item
        /// </summary>
        public string SINGLE_ERROR = "errorSingle";
    }

    public class FilterErrorCodes
    {
        /// <summary>
        /// Expected value missing
        /// </summary>
        public const string MISSING_VALUE = "errorFilterMissingValue";

        /// <summary>
        /// Filter operator not suported
        /// </summary>
        public const string OPERATOR_NOT_SUPPORTED = "errorFilterOperatorNotSupported";

        /// <summary>
        /// Incoherent filter value
        /// </summary>
        public const string INCOHERENT_VALUES = "errorFilterIncoherentValues";
    }
}
