namespace APIBaseTemplate.Common
{
    public static class Constants
    {
        public const string API_BASE_TEMPLATE_APPLICATION_NAME = "APIBaseTemplateDb";
        public const string API_BASE_TEMPLATE_APPLICATION_VERSION = "v1.0";
        public const string API_BASE_TEMPLATE_CONNECTIONSTRING_NAME = "APIBaseTemplateDb";
    }

    public static class HeaderConstants
    {
        /// <summary>
        /// This custom header allow to 'tie with a thread' different request/response if needed.
        /// </summary>
        /// <example>
        /// If a client to perform an operation needs to call two or more services, it set a value for this header between all calls.
        /// The server, if receive this header in request it will echo it in response.
        /// 
        /// If server doesn't receive this header will generate a new value.
        /// </example>
        public const string DISTRIBUITED_CONTEXT_ID_HEADER_NAME = "DIST-CTX-ID";
    }

    public static class ErrorConstants
    {
        /// <summary>
        /// Argument out of range.
        /// </summary>
        public const string OUT_OF_RANGE_ARGUMENT = "errorArgumentOutOfRange";

        /// <summary>
        /// Operation performed not valid.
        /// </summary>
        public const string INVALID_OPERATION = "errorInvalidOperation";

        /// <summary>
        /// Argument null.
        /// </summary>
        public const string MISSING_REQUIRED_INPUT_DATA = "errorMissingRequiredInputData";

        /// <summary>
        /// Invalid input data.
        /// </summary>
        public const string INVALID_INPUT_DATA = "errorInvalidInputData";
    }
}
