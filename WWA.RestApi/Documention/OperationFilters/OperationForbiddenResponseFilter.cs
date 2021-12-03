using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WWA.RestApi.Documention.OperationFilters
{
    public class OperationForbiddenResponseFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var attributes = context.MethodInfo.GetCustomAttributes(true).Concat(context.MethodInfo.DeclaringType.GetCustomAttributes(true));
            if (attributes.Any(a => a.GetType() == typeof(AuthorizeAttribute)))
                operation.Responses.Add("403", new OpenApiResponse { Description = "Not authorized to make this request." });
        }
    }
}
