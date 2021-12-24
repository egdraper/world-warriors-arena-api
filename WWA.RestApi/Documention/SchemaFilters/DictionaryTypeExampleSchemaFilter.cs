using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using WWA.RestApi.ViewModels.Maps;

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

            // Maps

            if (type == typeof(Dictionary<string, CellViewModel>))
            {
                schema.Example = new OpenApiObject
                {
                    ["0:0"] = new OpenApiObject
                    {
                        ["spriteId"] = new OpenApiString("string"),
                        ["tileLocation"] = new OpenApiObject
                        {
                            ["x"] = new OpenApiInteger(0),
                            ["y"] = new OpenApiInteger(0)
                        },
                        ["z"] = new OpenApiInteger(0),
                        ["isObstructed"] = new OpenApiBoolean(true)
                    },
                    ["0:1"] = new OpenApiObject
                    {
                        ["spriteId"] = new OpenApiString("string"),
                        ["tileLocation"] = new OpenApiObject
                        {
                            ["x"] = new OpenApiInteger(0),
                            ["y"] = new OpenApiInteger(0)
                        },
                        ["z"] = new OpenApiInteger(0),
                        ["isObstructed"] = new OpenApiBoolean(true)
                    }
                };
            }

            if (type == typeof(Dictionary<string, GatewayViewModel>))
            {
                schema.Example = new OpenApiObject
                {
                    ["0:0"] = new OpenApiObject
                    {
                        ["mapId"] = new OpenApiString("string"),
                        ["mapCoordinate"] = new OpenApiObject
                        {
                            ["x"] = new OpenApiInteger(0),
                            ["y"] = new OpenApiInteger(0)
                        }
                    },
                    ["0:1"] = new OpenApiObject
                    {
                        ["mapId"] = new OpenApiString("string"),
                        ["mapCoordinate"] = new OpenApiObject
                        {
                            ["x"] = new OpenApiInteger(0),
                            ["y"] = new OpenApiInteger(0)
                        }
                    }
                };
            }
        }
    }
}
