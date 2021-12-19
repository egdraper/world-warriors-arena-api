using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Writers;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.IO;
using System.Reflection;
using WWA.RestApi.Documention.DocumentFilters;
using WWA.RestApi.Documention.OperationFilters;
using WWA.RestApi.Documention.ParameterFilters;
using WWA.RestApi.Documention.SchemaFilters;

namespace WWA.RestApi.Documention
{
    public class LogoConfig : IOpenApiExtension
    {
        public const string Section = "logo";

        public string Url { get; set; }
        public string BackgroundColor { get; set; }

        public void Write(IOpenApiWriter writer, OpenApiSpecVersion specVersion)
        {
            writer.WriteStartObject();
            writer.WriteProperty("url", Url);
            writer.WriteProperty("backgroundColor", BackgroundColor);
            writer.WriteEndObject();
        }
    }

    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IConfiguration _config;

        public ConfigureSwaggerOptions(IConfiguration config)
        {
            _config = config;
        }

        public void Configure(SwaggerGenOptions options)
        {
            var logoConfig = new LogoConfig();
            _config.GetSection(LogoConfig.Section).Bind(logoConfig);

            options.SwaggerDoc("v1", new()
                {
                    Title = "World Warriors Arena REST API",
                    Version = "v1",
                    Description = "<cool description goes here>",
                    Extensions = { ["x-logo"] = logoConfig }
                }
            );

            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

            options.AddSecurityDefinition("Access Token", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Access Token"
                },
                Scheme = "bearer",
                BearerFormat = "JWT"
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Access Token" }
                    },
                    new string[] { "wwa_restapi" }
                }
            });

            options.EnableAnnotations();

            // operation filters.
            options.OperationFilter<OperationBadRequestResponseFilter>();
            options.OperationFilter<OperationForbiddenResponseFilter>();
            options.OperationFilter<OperationNoContentResponseFilter>();
            options.OperationFilter<OperationNotFoundResponseFilter>();
            options.OperationFilter<OperationRequestContentFilter>();
            options.OperationFilter<OperationResourceNameFilter>();
            options.OperationFilter<OperationResponseContentFilter>();
            options.OperationFilter<OperationUnauthorizedResponseFilter>();

            // parameter filters.
            options.ParameterFilter<ParameterStyleFilter>();

            // schema filters.
            options.SchemaFilter<DictionaryTypeExampleSchemaFilter>();
            options.SchemaFilter<PatchOperationSchemaFilter>();
            options.SchemaFilter<RemoveFalseAdditionalPropertiesForAutorestSchemaFilter>();

            // document filters.
            options.DocumentFilter<TagsDocumentFilter>();

            options.MapType<FileContentResult>(() => new OpenApiSchema { Type = "file" });
        }
    }
}
