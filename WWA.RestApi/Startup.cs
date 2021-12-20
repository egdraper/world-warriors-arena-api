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
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Orleans;
using Orleans.Configuration;
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
using WWA.Grains.Games;
using WWA.Grains.Maps;
using WWA.Grains.Mongo;
using WWA.Grains.Users;
using WWA.RestApi.Documention;

namespace WWA.RestApi
{
    public class Startup
    {
        
        public IConfiguration Configuration;
        public static ApiConfig Config;
        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();

            services.Configure<ApiConfig>(Configuration);
            Config = Configuration.Get<ApiConfig>();

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
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver =
                        new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.Converters.Add(new StringEnumConverter
                    {
                        NamingStrategy = new CamelCaseNamingStrategy()
                    });
                })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                });

            services
                .AddMvcCore(options =>
                {
                    options.ReturnHttpNotAcceptable = true;
                    options.RespectBrowserAcceptHeader = true;
                });

            services.AddCors();
            services.AddControllers(options =>
            {
                options.InputFormatters.Insert(0, GetJsonPatchInputFormatter());
            });

            services.AddAutoMapper(new Type[]
            {
                typeof(AutoMapperProfile),
                typeof(Grains.Games.AutoMapperProfile),
                typeof(Grains.Maps.AutoMapperProfile),
                typeof(Grains.Users.AutoMapperProfile)
            });

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen();

            services.Configure<ClusterMembershipOptions>(options =>
            {
                options.DefunctSiloCleanupPeriod = TimeSpan.FromHours(1);
                options.DefunctSiloExpiration = TimeSpan.FromDays(1);
            });
            services.Configure<ClusterOptions>(options =>
            {
                options.ClusterId = Configuration.GetValue<string>("orleans:clusterId");
                options.ServiceId = "wwa.silo";
            });
            services.Configure<ConsoleLifetimeOptions>(options =>
            {
                options.SuppressStatusMessages = true;
            });
            services.Configure<SiloOptions>(options =>
            {
                options.SiloName = Environment.MachineName;
            });
            services.Configure<MongoConfiguration>(Configuration.GetSection("mongo"));
            services.AddHttpClient();
        }

        private static NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter()
        {
            var builder = new ServiceCollection()
                .AddLogging()
                .AddMvc()
                .AddNewtonsoftJson()
                .Services.BuildServiceProvider();

            return builder
                .GetRequiredService<IOptions<MvcOptions>>()
                .Value
                .InputFormatters
                .OfType<NewtonsoftJsonPatchInputFormatter>()
                .First();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {

            builder.RegisterInstance(Configuration).AsImplementedInterfaces();

            // Grains
            builder.RegisterType<GameService>().AsSelf();
            builder.RegisterType<GameGrain>().AsSelf();
            builder.RegisterType<MapService>().AsSelf();
            builder.RegisterType<MapGrain>().AsSelf();
            builder.RegisterType<UserService>().AsSelf();
            builder.RegisterType<UserGrain>().AsSelf();

            // Repositories
            builder.RegisterType<MongoContext>().As<IMongoContext>().SingleInstance();
            builder.RegisterType<GameRepository>().As<IGameRepository>().SingleInstance();
            builder.RegisterType<MapRepository>().As<IMapRepository>().SingleInstance();
            builder.RegisterType<UserRepository>().As<IUserRepository>().SingleInstance();

            RegisterSingletons(builder);
        }

        public void Configure(IApplicationBuilder app, IHostApplicationLifetime host)
        {

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
        }
    }
}
