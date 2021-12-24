using Autofac.Extensions.DependencyInjection;
using Common.Logging;
using Common.Logging.Serilog;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Newtonsoft.Json;
using Orleans;
using Orleans.Hosting;
using Orleans.Providers.MongoDB.Configuration;
using Orleans.Statistics;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using WWA.Grains.Constants;
using WWA.Grains.Games;
using WWA.Grains.Maps;
using WWA.Grains.Users;
using WWA.RestApi.Filters;

namespace WWA.RestApi
{
    internal class Program
    {
        static Program()
        {
            Log.Logger = new LoggerConfiguration()
#if DEBUG
                .MinimumLevel.Debug()
                .WriteTo.Async(a => a.File(
                    new JsonFormatter(),
                    @"C:\Logs\WWA.RestApi.{Date}.log",
                    retainedFileCountLimit: 5))
#endif
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Orleans", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Async(a => a.Console(
                    outputTemplate: "[{Timestamp:s} {Level:u3}] {Properties} {Message:lj}{NewLine}{Exception}", theme: ConsoleTheme.None))
                .CreateLogger();
            LogManager.Adapter = new SerilogFactoryAdapter(Log.Logger);
        }
        private static async Task Main(string[] args)
        {
            Log.Information("Starting service...");
            try
            {
                await Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    config
                        .SetBasePath(context.HostingEnvironment.ContentRootPath)
                        .AddJsonFile("config.json", true)
                        .AddJsonFile($@"{context.HostingEnvironment.ContentRootPath}\..\config.json", optional: true)
                        .AddJsonFile(Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".wwa", "config.json"), true)
                        .AddEnvironmentVariables()
                        .AddCommandLine(args);
                })
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .UseSerilog()
                .UseOrleans((context, builder) =>
                {
                    var mongoUrlBuilder = new MongoUrlBuilder(context.Configuration.GetValue<string>("mongo:connectionString"));
                    builder.UseMongoDBClient(context.Configuration.GetValue<string>("mongo:connectionString"));
                    builder.UseMongoDBClustering(options =>
                    {
                        options.DatabaseName = mongoUrlBuilder.DatabaseName ?? "wwa";
                        options.Strategy = MongoDBMembershipStrategy.SingleDocument;
                    });
                    builder.AddMongoDBGrainStorage(MongoStorageProviders.GrainState, options =>
                    {
                        options.DatabaseName = mongoUrlBuilder.DatabaseName ?? "wwa";
                        options.CreateShardKeyForCosmos = false;
                        options.CollectionPrefix = "";

                        options.ConfigureJsonSerializerSettings = settings =>
                        {
                            settings.NullValueHandling = NullValueHandling.Include;
                            settings.ObjectCreationHandling = ObjectCreationHandling.Replace;
                            settings.DefaultValueHandling = DefaultValueHandling.Populate;
                        };

                    });
                    builder.AddMongoDBGrainStorage(MongoStorageProviders.GrainStreamSubscriptions, options =>
                    {
                        options.Configure(o =>
                        {
                            o.DatabaseName = mongoUrlBuilder.DatabaseName ?? "wwa";
                            o.CollectionPrefix = "Orleans";
                            o.ConfigureJsonSerializerSettings = settings =>
                            {
                                settings.NullValueHandling = NullValueHandling.Include;
                                settings.ObjectCreationHandling = ObjectCreationHandling.Replace;
                                settings.DefaultValueHandling = DefaultValueHandling.Populate;
                            };
                        });
                    });
                    builder.AddMongoDBGrainStorageAsDefault(options =>
                    {
                        options.Configure(o =>
                        {
                            o.DatabaseName = mongoUrlBuilder.DatabaseName ?? "wwa";
                            o.CollectionPrefix = "";
                            o.ConfigureJsonSerializerSettings = settings =>
                            {
                                settings.NullValueHandling = NullValueHandling.Include;
                                settings.ObjectCreationHandling = ObjectCreationHandling.Replace;
                                settings.DefaultValueHandling = DefaultValueHandling.Populate;
                            };
                        });
                    });
                    builder.AddIncomingGrainCallFilter<LoggingCallFilter>();
                    builder.UseDashboard(options => { });
                    builder.UseLinuxEnvironmentStatistics();

                    // TODO: In order to support multiple hosts forming a cluster, they must listen on different ports.
                    //       Need to replace this with something better for autoscaling replica sets
                    var instanceId = context.Configuration.GetValue<int>("orleans:instanceId");
                    builder.UseLocalhostClustering(
                        siloPort: 11111 + instanceId,
                        gatewayPort: 30000 + instanceId,
                        primarySiloEndpoint: new IPEndPoint(IPAddress.Loopback, 11111)
                    );
                    builder.ConfigureApplicationParts(parts =>
                        parts
                            .AddApplicationPart(typeof(GameService).Assembly)
                            .AddApplicationPart(typeof(GameGrain).Assembly)
                            .AddApplicationPart(typeof(WorldMapService).Assembly)
                            .AddApplicationPart(typeof(WorldMapGrain).Assembly)
                            .AddApplicationPart(typeof(UserService).Assembly)
                            .AddApplicationPart(typeof(UserGrain).Assembly)
                            .WithReferences()
                    );
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .RunConsoleAsync();
            }
            catch (Exception e)
            {
                Log.Fatal(e, e.Message);
                throw;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
