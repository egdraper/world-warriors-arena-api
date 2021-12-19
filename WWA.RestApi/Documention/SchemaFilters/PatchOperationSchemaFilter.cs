using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WWA.RestApi.Documention.SchemaFilters
{
    public class PatchOperationSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            var type = context.Type;

            // Dictionary string, string

            if (type == typeof(Operation))
            {
                schema.Example = new OpenApiObject
                {
                    ["path"] = new OpenApiString("/property"),
                    ["op"] = new OpenApiString("replace"),
                    ["value"] = new OpenApiString("value")
                };
            }
        }
    }
}
