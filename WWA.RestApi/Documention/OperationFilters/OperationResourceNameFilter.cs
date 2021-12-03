using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace WWA.RestApi.Documention.OperationFilters
{
    public class OperationResourceNameFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var resourceName = new ApiResourceName(context.ApiDescription.RelativePath);
            operation.OperationId = context.MethodInfo.Name.Replace("Async", "");
            var action = operation.OperationId.Humanize(LetterCasing.Title).Split(' ')[0].Pluralize();
            if (context.MethodInfo.GetCustomAttributes(true).OfType<HttpGetAttribute>().Any() && !resourceName.IsSingular)
            {
                operation.Summary = $"{action} a list of {resourceName.Names}";
            }
            else
            {
                operation.Summary = $"{action} {resourceName.Article} {resourceName.Name}";
            }                       

            foreach (var responseKV in operation.Responses.Where(r => r.Key == "200"))
            {
                var response = responseKV.Value;
                if (resourceName.IsSingular)
                {
                    response.Description = $"Successfully retrieved {resourceName.Article} {resourceName.Name}.";
                    continue;
                }
                
                var isPaged = context.ApiDescription.ParameterDescriptions.Any(param =>
                    (param.Source == BindingSource.Query || param.Source == BindingSource.ModelBinding) && param.Name == "Skip");

                var paged = " ";
                if (isPaged)
                {
                    paged = "paged ";
                    response.Headers = new Dictionary<string, OpenApiHeader>
                    {
                        ["X-Total-Count"] = new OpenApiHeader
                        {
                            Description = "The total number of filtered resources.",
                            Schema = new OpenApiSchema { Type = "number" }
                        }
                    };
                }

                response.Description = $"Successfully retrieved a list of {paged}{resourceName.Names}.";
            }
            
            operation.Responses.Add("500", new OpenApiResponse { Description = "There was an internal server error." });
        }
    }
}
