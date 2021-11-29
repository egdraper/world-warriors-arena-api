using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WWA.RestApi.Documention.OperationFilters
{
    public class OperationNoContentResponseFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var httpMethod = context.ApiDescription.HttpMethod;

            if (httpMethod != "DELETE" && httpMethod != "HEAD") return;

            // If this is a DELETE or HEAD request, then no content (204) should be returned instead of ok (200)
            operation.Responses.Remove("200");
            operation.Responses.Add("204", new OpenApiResponse
            {
                Description = "Success"
            });
        }
    }
}
