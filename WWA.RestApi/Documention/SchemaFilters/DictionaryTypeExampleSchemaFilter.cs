using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace WWA.RestApi.Documention.SchemaFilters
{
    public class DictionaryTypeExampleSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            var type = context.Type;

            // Dictionary string, string

            if (type == typeof(Dictionary<string, string>))
            {
                schema.Example = new OpenApiObject
                {
                    ["key1"] = new OpenApiString("value1"),
                    ["key2"] = new OpenApiString("value2")
                };
            }

            // Dictionary string, double

            if (type == typeof(Dictionary<string, double>))
            {
                schema.Example = new OpenApiObject
                {
                    ["key1"] = new OpenApiDouble(1.0),
                    ["key2"] = new OpenApiDouble(2.5)
                };
            }
        }
    }
}
