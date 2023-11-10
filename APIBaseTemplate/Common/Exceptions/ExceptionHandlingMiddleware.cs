using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.Extensions.Logging.Abstractions;

namespace APIBaseTemplate.Common
{
    /// <summary>
    /// A middleware that handles exceptions
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerFactory _loggerFactory;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILoggerFactory loggerFactory)
        {
            _next = next;
            _loggerFactory = loggerFactory ?? NullLoggerFactory.Instance;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                var logger = _loggerFactory.CreateLogger<ExceptionHandlingMiddleware>();

                var distribuitedContextId = context.GetDistribuitedContextIdHeader();
                var errorDescriptor = ErrorDescriptorHelper.FromException(ex, distribuitedContextId);
                string errorParamsString = ErrorDescriptorHelper.GetExceptionPublicAndPrivateErrorCodeParameters(ex);
                using (logger.BeginScope("Error while processing request {requestPath}", context.Request.Path))
                {
                    logger.LogError(ex, $"Error encountered|Message: '{ex.Message}'|Error parameters: {errorParamsString}|Distribuited context id: {distribuitedContextId}|Error descriptor: {errorDescriptor}");

                    IActionResult result = new JsonResult(errorDescriptor)
                    {
                        StatusCode = ErrorDescriptorHelper.GetHttpStatusCode(ex)
                    };

                    RouteData routeData = context.GetRouteData();
                    ActionDescriptor actionDescriptor = new ActionDescriptor();
                    ActionContext actionContext = new ActionContext(context, routeData, actionDescriptor);

                    await result.ExecuteResultAsync(actionContext);
                }
                return;
            }
        }
    }
}
