using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

namespace WWA.RestApi.Documention.OperationFilters
{
    public class OperationBadRequestResponseFilter : IOperationFilter
    {        
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var descriptions = context.ApiDescription.ParameterDescriptions;

            // If there are query/body parameters, then validation errors (400) can be returned.
            if (descriptions.Any(p => p.Source == BindingSource.Query || p.Source == BindingSource.Body))
                operation.Responses.Add("400", new OpenApiResponse { Description = "There were validation errors." });
        }
    }
}
