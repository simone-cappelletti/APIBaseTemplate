using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Net;

namespace APIBaseTemplate.Services
{
    public class ProducesResponseTypeConvention : IApplicationModelConvention
    {
        private List<Type> _voidActionResults = new List<Type>() { typeof(void), typeof(Task) };

        public void Apply(ApplicationModel application)
        {
            var actions = application.Controllers.SelectMany(c => c.Actions);

            foreach (var action in actions)
            {
                // Default attribute
                action.Filters.Add(new ProducesResponseTypeAttribute((int)HttpStatusCode.Unauthorized));
                action.Filters.Add(new ProducesResponseTypeAttribute((int)HttpStatusCode.Forbidden));

                // Void or Task return type
                if (_voidActionResults.Contains(action.ActionMethod.ReturnType))
                {
                    action.Filters.Add(new ProducesResponseTypeAttribute((int)HttpStatusCode.NoContent));
                }
                // All other return type
                else
                {
                    var httpMethodAttribute = action.Attributes.OfType<HttpMethodAttribute>().FirstOrDefault();

                    switch (httpMethodAttribute?.HttpMethods.FirstOrDefault())
                    {
                        case "GET":

                            action.Filters.Add(new ProducesResponseTypeAttribute((int)HttpStatusCode.OK));
                            action.Filters.Add(new ProducesResponseTypeAttribute((int)HttpStatusCode.NotFound));

                            break;

                        case "POST":

                            action.Filters.Add(new ProducesResponseTypeAttribute((int)HttpStatusCode.OK));
                            action.Filters.Add(new ProducesResponseTypeAttribute((int)HttpStatusCode.BadRequest));

                            break;

                        case "PUT":

                            break;
                        case "DELETE":

                            break;
                    }
                }
            }
        }
    }
}
