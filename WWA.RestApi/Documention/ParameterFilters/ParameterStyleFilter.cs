using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WWA.RestApi.Documention.ParameterFilters
{
    public class ParameterStyleFilter : IParameterFilter
    {
        public void Apply(OpenApiParameter parameter, ParameterFilterContext context)
        {
            if (parameter.Schema.Type != "array" && parameter.Schema.Type != "object")
                return;

            parameter.Explode = true;
            switch (parameter.In)
            {
                case ParameterLocation.Query:
                    if (parameter.Schema.Type == "array")
                    {
                        parameter.Style = ParameterStyle.Form;
                        parameter.Explode = false;
                    }
                    else if (parameter.Schema.Type == "object")
                    {
                        parameter.Style = ParameterStyle.DeepObject;
                    }
                    break;
                case ParameterLocation.Cookie:
                    parameter.Style = ParameterStyle.Form;
                    parameter.Explode = false;
                    break;
                case ParameterLocation.Path:
                case ParameterLocation.Header:
                    parameter.Style = ParameterStyle.Simple;
                    break;
            }
        }
    }
}
