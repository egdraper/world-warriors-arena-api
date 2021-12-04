using Audit.MongoDB.Providers;
using Autofac;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using Orleans;
using Orleans.Configuration;
using Orleans.Providers.MongoDB.Configuration;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WWA.Configuration;
using WWA.RestApi.Documention;
using WWA.RestApi.HostedServices;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace WWA.RestApi
{
    public class Startup
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger _logger;
        
        public IConfiguration Configuration;
        public static ApiConfig Config;
        
        public Startup(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            Configuration = configuration;
            _loggerFactory = loggerFactory;
            _logger = _loggerFactory.CreateLogger<Startup>();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            _logger.LogInformation("Configuring services...");

            services.AddCors();
            services.AddControllers();

            services.AddOptions();

            services.Configure<ApiConfig>(Configuration);
            Config = Configuration.Get<ApiConfig>();

            services.AddAutoMapper(typeof(AutoMapperProfile));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer("WWA", options =>
                {
                    options.Audience = Config.Identity.Audience;
                    options.RequireHttpsMetadata = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = Config.Identity.Issuer,
                        ValidateIssuer = true,
                        ValidAudience = Config.Identity.Audience,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        // TODO: Switch to a cert later and then sign with new X509SecurityKey(new X509Certificate2(rawData)).
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Config.Identity.Secret)),
                        ValidateIssuerSigningKey = true
                };
                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            Console.WriteLine("OnAuthenticationFailed: " +
                                context.Exception.Message);
                            return Task.CompletedTask;
                        },
                        OnTokenValidated = context =>
                        {
                            Console.WriteLine("OnTokenValidated: " +
                                context.SecurityToken);
                            return Task.CompletedTask;
                        }
                    };
                });
            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .AddAuthenticationSchemes(new[] { "WWA" })
                    .RequireClaim("scope", "wwa_restapi")
                    .Build();
            });
            services
                .AddMvc(options =>
                {
                    options.ReturnHttpNotAcceptable = true;
                    options.EnableEndpointRouting = false;
                    options.OutputFormatters.RemoveType<TextOutputFormatter>();
                    options.OutputFormatters.RemoveType<HttpNoContentOutputFormatter>();
                })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    options.JsonSerializerOptions.IgnoreNullValues = true;
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services
                .AddMvcCore(options =>
                {
                    options.ReturnHttpNotAcceptable = true;
                    options.RespectBrowserAcceptHeader = true;
                });

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen();
            services.AddHostedService<ClusterClientService>();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            _logger.LogInformation("Configuring dependency injection...");

            builder.RegisterInstance(Configuration).AsImplementedInterfaces();

            RegisterSingletons(builder);
        }

        public void Configure(IApplicationBuilder app, IHostApplicationLifetime host)
        {
            _logger.LogInformation("Configuring application...");

#if DEBUG
            app.UseDeveloperExceptionPage();
#endif
            app.UseRouting();
            // global cors policy
            app.UseCors(c => c
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());

            var mongo = Configuration.GetValue<string>("mongo:connectionString");
            Audit.Core.Configuration.DataProvider = new MongoDataProvider
            {
                ConnectionString = mongo,
                Database = new MongoUrl(mongo).DatabaseName,
                Collection = "audit"
            };

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSwagger(options =>
            {
                options.RouteTemplate = "docs/{documentName}/swagger.json";
            });
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint($"/docs/v1/swagger.json", "WWA.RestApi v1");
                options.RoutePrefix = "docs";
                options.InjectStylesheet("./swashbuckle/ui/custom.css");
                options.InjectJavascript("./swashbuckle/ui/custom.js");
                options.DocExpansion(DocExpansion.None);
                options.DefaultModelExpandDepth(15);
                options.DefaultModelRendering(ModelRendering.Example);
                options.DefaultModelsExpandDepth(-1);
                options.DisplayOperationId();
                options.DisplayRequestDuration();
                options.EnableDeepLinking();
                options.EnableFilter();
            });

            app.UseSerilogRequestLogging();
            app.UseMvc();

            host.ApplicationStopped.Register(Log.CloseAndFlush);
        }

        private void RegisterSingletons(ContainerBuilder builder)
        {
            _logger.LogInformation("Registering singletons...");

            builder
                .Register(options => new MapperConfiguration(c =>
                {
                    c.AddMaps(AppDomain.CurrentDomain.GetAssemblies().Where(a => a.FullName.StartsWith("WWA")));
                }))
                .As<AutoMapper.IConfigurationProvider>()
                .SingleInstance();
            builder
                .Register(d => new Mapper(d.Resolve<AutoMapper.IConfigurationProvider>(), d.Resolve<ILifetimeScope>().Resolve))
                .As<IMapper>()
                .InstancePerLifetimeScope();
            builder.Register(context =>
            {
                var mongoUrlBuilder = new MongoUrlBuilder(Configuration.GetValue<string>("mongo:connectionString"));
                var logger = context.Resolve<ILogger<Startup>>();
                var clusterClient = new ClientBuilder()
                    .Configure<ClusterOptions>(options =>
                    {
                        options.ClusterId = Configuration.GetValue<string>("orleans:clusterId");
                        options.ServiceId = "restapi";
                    })
                    .UseMongoDBClient(Configuration.GetValue<string>("mongo:connectionString"))
                    .UseMongoDBClustering(options =>
                    {
                        options.DatabaseName = mongoUrlBuilder.DatabaseName ?? "wwa";
                        options.Strategy = MongoDBMembershipStrategy.SingleDocument;
                    })
                    .Build();
                return clusterClient;
            }).As<IClusterClient>().SingleInstance();
        }
    }
}
