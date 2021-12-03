using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using WWA.RestApi.Constants;

namespace WWA.RestApi.Documention.OperationFilters
{
    public class OperationRequestContentFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.RequestBody?.Content?.ContainsKey(ContentTypes.AppJson) ?? false)
            {
                operation.RequestBody.Content = new Dictionary<string, OpenApiMediaType>
                {
                    [ContentTypes.AppJson] = operation.RequestBody.Content[ContentTypes.AppJson]
                };
            }
        }
    }
}
