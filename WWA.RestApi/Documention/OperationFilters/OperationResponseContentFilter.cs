using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;
using WWA.RestApi.Constants;

namespace WWA.RestApi.Documention.OperationFilters
{
    public class OperationResponseContentFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var producesAttributes = context.MethodInfo.GetCustomAttributes(true).OfType<ProducesAttribute>();

            var allowContentTypes = producesAttributes.Any()
                ? new HashSet<string>(producesAttributes.SelectMany(s => s.ContentTypes))
                : new HashSet<string> { ContentTypes.AppJson };

            foreach (var response in operation.Responses.Values)
            {
                foreach (var contentType in response.Content.Keys.Except(allowContentTypes).ToList())
                {
                    response.Content.Remove(contentType);
                }
            }
        }
    }
}
