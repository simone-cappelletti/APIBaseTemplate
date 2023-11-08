using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Net;

namespace APIBaseTemplate.Services
{
    public class ProducesResponseTypeConvention : IApplicationModelConvention
    {
        private List<Type> _voidActionResults = new List<Type>() { typeof(void), typeof(Task) };

        private const string HTTP_METHOD_GET = "GET";
        private const string HTTP_METHOD_POST = "POST";
        private const string HTTP_METHOD_PUT = "PUT";
        private const string HTTP_METHOD_DELETE = "DELETE";

        public void Apply(ApplicationModel application)
        {
            var actions = application.Controllers.SelectMany(c => c.Actions);

            foreach (var action in actions)
            {
                // Default attribute
                action.Filters.Add(new ProducesResponseTypeAttribute((int)HttpStatusCode.InternalServerError));
                action.Filters.Add(new ProducesResponseTypeAttribute((int)HttpStatusCode.Unauthorized));
                action.Filters.Add(new ProducesResponseTypeAttribute((int)HttpStatusCode.Forbidden));
                action.Filters.Add(new ProducesResponseTypeAttribute((int)HttpStatusCode.OK));

                // Void or Task return type
                if (_voidActionResults.Contains(action.ActionMethod.ReturnType))
                {
                    //action.Filters.Add(new ProducesResponseTypeAttribute((int)HttpStatusCode.NoContent));
                }

                // All other return type
                else
                {
                    var httpMethodAttribute = action.Attributes.OfType<HttpMethodAttribute>().Single();

                    switch (httpMethodAttribute?.HttpMethods.FirstOrDefault())
                    {
                        case HTTP_METHOD_GET:

                            action.Filters.Add(new ProducesResponseTypeAttribute((int)HttpStatusCode.NotFound));

                            break;

                        case HTTP_METHOD_POST:

                            action.Filters.Add(new ProducesResponseTypeAttribute((int)HttpStatusCode.NotFound));

                            break;

                        case HTTP_METHOD_PUT:

                            break;

                        case HTTP_METHOD_DELETE:

                            action.Filters.Add(new ProducesResponseTypeAttribute((int)HttpStatusCode.NotFound));

                            break;
                    }
                }
            }
        }
    }
}
