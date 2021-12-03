using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

namespace WWA.RestApi.Documention.OperationFilters
{
    public class OperationNotFoundResponseFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var descriptions = context.ApiDescription.ParameterDescriptions;

            if (!descriptions.Any(p => p.Source == BindingSource.Path)) return;

            // If there are path parameters, then not found (404) errors can be returned.
            operation.Responses.Add("404", new OpenApiResponse 
            { 
                Description = "The target route does not exist or contains an ID that cannot be found." 
            });
        }
    }
}
