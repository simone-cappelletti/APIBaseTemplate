namespace APIBaseTemplate.Common
{
    /// <summary>
    /// Describe an error encountered
    /// </summary>
    public class ErrorDescriptor
    {
        public ErrorDescriptor(
            string errorCaseId,
            string distribuitedContextId,
            string errorCode,
            string? errorOrigin = null,
            IDictionary<string, string>? errorMessageParams = null) =>
        (
            ErrorCaseId,
            DistribuitedContextId,
            ErrorCode,
            ErrorOrigin,
            ErrorMessageParams
        ) = (
            errorCaseId,
            distribuitedContextId,
            errorCode,
            errorOrigin,
            errorMessageParams ?? new Dictionary<string, string>()
        );

        /// <summary>
        /// An identifier for this error case
        /// </summary>
        /// <remarks>
        /// Helps to this incident among application log
        /// </remarks>
        public string ErrorCaseId { get; set; }

        /// <summary>
        /// Distributed context identifier
        /// </summary>
        /// <remarks>
        /// A client-generated identifier which eventually ties together several server requests
        /// </remarks>
        public string DistribuitedContextId { get; set; }

        /// <summary>
        /// A string error code which is also a translation key
        /// </summary>
        /// <example>
        /// itemNotFound
        /// </example>
        public string ErrorCode { get; set; }

        /// <summary>
        /// Information about error origin
        /// </summary>
        public string? ErrorOrigin { get; set; }

        /// <summary>
        /// Parameters for the translated message described by the key <see cref="ErrorCode"/>
        /// </summary>
        public IDictionary<string, string> ErrorMessageParams { get; set; }

        public override string ToString()
        {
            string ErrorMessageParamsString = string.Empty;

            if (ErrorMessageParams != null)
            {
                ErrorMessageParamsString = string.Join(
                    ',',
                    ErrorMessageParams.Select(p => $"{p.Key}:'{p.Value}'"));
            }

            return $"[{nameof(ErrorDescriptor)}|{nameof(ErrorCaseId)}:'{ErrorCaseId}', {nameof(DistribuitedContextId)}:'{DistribuitedContextId}', {nameof(ErrorCode)}:'{ErrorCode}', {nameof(ErrorOrigin)}:'{ErrorOrigin}', {nameof(ErrorMessageParams)}:{ErrorMessageParamsString}]";
        }
    }
}
