using System.Net;
using System.Security.Cryptography;

namespace APIBaseTemplate.Common
{
    internal class ErrorDescriptorHelper
    {
        /// <summary>
        /// Initialize an <see cref="ErrorDescriptor"/> from exception <paramref name="exc"/>
        /// </summary>
        /// <param name="exc"></param>
        /// <param name="distribuitedContextId">
        /// Distribuited context identifier
        /// </param>
        /// <param name="options"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static ErrorDescriptor FromException(
            Exception ex,
            string distribuitedContextId)
        {
            if (ex is null)
            {
                throw new ArgumentNullException(nameof(ex));
            }

            ErrorDescriptor errorDescriptor = new ErrorDescriptor(
                    errorCaseId: Guid.NewGuid().ToString(),
                    distribuitedContextId: distribuitedContextId,
                    errorCode: GenericErrorCodes.UNEXPECTED,
                    errorOrigin: $"{ex.TargetSite?.DeclaringType?.Name}_{ex.TargetSite?.Name}");

            if (ex is BaseException baseExc)
            {
                errorDescriptor.ErrorCode = baseExc.ErrorCode;

                if (baseExc.PublicErrorCodeParameters != null &&
                    baseExc.PublicErrorCodeParameters.Count > 0)
                {
                    foreach (var key in baseExc.PublicErrorCodeParameters.Keys)
                    {
                        errorDescriptor.ErrorMessageParams.Add(key, baseExc.PublicErrorCodeParameters[key]?.ToString());
                    }
                }
            }
            else if (ex is ArgumentOutOfRangeException argumentOutOfRangeException)
            {
                if (!string.IsNullOrEmpty(argumentOutOfRangeException.ParamName))
                {
                    errorDescriptor.ErrorMessageParams.Add("parameterName", argumentOutOfRangeException.ParamName);
                }
                errorDescriptor.ErrorCode = ErrorConstants.OUT_OF_RANGE_ARGUMENT;
            }
            else if (ex is InvalidOperationException)
            {
                errorDescriptor.ErrorCode = ErrorConstants.INVALID_OPERATION;
            }
            else if (ex is ArgumentNullException argNullExc)
            {
                errorDescriptor.ErrorCode = ErrorConstants.MISSING_REQUIRED_INPUT_DATA;
                errorDescriptor.ErrorMessageParams.Add("nullParameterName", argNullExc.ParamName);
            }
            else if (ex is ArgumentException argExc)
            {
                errorDescriptor.ErrorCode = ErrorConstants.INVALID_INPUT_DATA;
                errorDescriptor.ErrorMessageParams.Add("parameterName", argExc.ParamName);
            }

            return errorDescriptor;
        }

        /// <summary>
        /// Returns http status code for exception <paramref name="exc"/> otherwise a default value.
        /// </summary>
        /// <param name="exc"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static int GetHttpStatusCode(
        Exception exc)
        {
            if (exc is null)
            {
                throw new ArgumentNullException(nameof(exc));
            }

            var httpStatusCode = HttpStatusCode.InternalServerError;

            if (exc is BaseException baseExc)
            {
                if (baseExc.HttpStatus.HasValue)
                {
                    httpStatusCode = baseExc.HttpStatus.Value;
                }
            }
            else if (exc is CryptographicException)
            {
                httpStatusCode = HttpStatusCode.BadRequest;
            }
            else if (
                exc is InvalidOperationException ||
                exc is ArgumentOutOfRangeException ||
                exc is ArgumentException ||
                exc is ArgumentNullException ||
                exc is InvalidOperationException)
            {
                httpStatusCode = HttpStatusCode.BadRequest;
            }

            return (int)httpStatusCode;
        }

        /// <summary>
        /// If <paramref name="ex"/> is <see cref="BaseException"/>, return the <see cref="IDictionary{string, string}"/> with 
        /// all <see cref="BaseException.PublicAndPrivateErrorCodeParameters"/>
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static string GetExceptionPublicAndPrivateErrorCodeParameters(Exception ex)
        {
            if (ex is null)
            {
                throw new ArgumentNullException(nameof(ex));
            }

            Dictionary<string, string> ret = new Dictionary<string, string>();

            if (ex is BaseException baseExc)
            {
                if (baseExc.PublicAndPrivateErrorCodeParameters != null && baseExc.PublicAndPrivateErrorCodeParameters.Count > 0)
                    foreach (var item in baseExc.PublicAndPrivateErrorCodeParameters)
                    {
                        ret.Add(item.Key, item.Value != null ? item.Value.ToString() : "");
                    }
            }

            if (ret == null)
            {
                return string.Empty;
            }
            else
            {
                return string.Join(',', ret.Select(i => $"\"{i.Key}\":\"{i.Value}\"").ToArray());
            }
        }
    }
}
