using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WWA.RestApi.Documention.SchemaFilters
{
    public class RemoveFalseAdditionalPropertiesForAutorestSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            // If AdditionalPropertiesAllowed is false and AdditionalProperties is null, Swashbuckle will set AdditionalProperites
            // to false. This is valid according to the OpenAPI spec, however, this will break AutoRest client generation.
            // This makes it so that AutoRest doesn't get broken by this behavior.
            if (schema.AdditionalProperties == null)
                schema.AdditionalPropertiesAllowed = true;
        }
    }
}
