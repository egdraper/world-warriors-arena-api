using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Common.Logging;
using Common.Logging.Serilog;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Newtonsoft.Json;
using Orleans;
using Orleans.Configuration;
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
using System.Net.Sockets;
using System.Threading.Tasks;
using WWA.Configuration;
using WWA.Grains.Constants;
using WWA.Grains.Mongo;
using WWA.Grains.Games;
using WWA.Grains.Users;
using WWA.Silo.Filters;

namespace WWA.Silo
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
                    @"C:\Logs\WWA.Silo.{Date}.log",
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

        private static Task Main(string[] args)
        {
            try
            {
                return new HostBuilder()
                    .ConfigureAppConfiguration((context, builder) =>
                    {
                        builder
                            .SetBasePath(context.HostingEnvironment.ContentRootPath)
                            .AddJsonFile("config.json", true)
                            .AddJsonFile($@"{context.HostingEnvironment.ContentRootPath}\..\config.json", optional: true)
                            .AddJsonFile(Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".wwa", "config.json"), true)
                            .AddEnvironmentVariables()
                            .AddCommandLine(args);
                    })
                    .ConfigureServices((context, services) =>
                    {
                        services.AddAutoMapper(typeof(Grains.Games.AutoMapperProfile));
                        services.AddAutoMapper(typeof(Grains.Users.AutoMapperProfile));

                        services.Configure<ClusterMembershipOptions>(options =>
                        {
                            options.DefunctSiloCleanupPeriod = TimeSpan.FromHours(1);
                            options.DefunctSiloExpiration = TimeSpan.FromDays(1);
                        });
                        services.Configure<ClusterOptions>(options =>
                        {
                            options.ClusterId = context.Configuration.GetValue<string>("orleans:clusterId");
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
                        services.Configure<MongoConfiguration>(context.Configuration.GetSection("mongo"));
                        services.AddHttpClient();
                    })
                    .UseSerilog()
                    .UseServiceProviderFactory(new AutofacServiceProviderFactory(ConfigureContainer))
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
                        builder.AddSimpleMessageStreamProvider("SmsProvider", options =>
                         {
                             options.FireAndForgetDelivery = true;
                             options.OptimizeForImmutableData = true;
                             options.PubSubType = Orleans.Streams.StreamPubSubType.ExplicitGrainBasedOnly;
                         });
                        builder.UseDashboard(options => { });
                        builder.UseLinuxEnvironmentStatistics();
#if DEBUG
                        builder.ConfigureEndpoints(
                            IPAddress.Loopback,
                            GetAvailablePort(11111, 11131),
                            GetAvailablePort(30000, 30020),
                            true);
#else
                        builder.ConfigureEndpoints(
                            GetAvailablePort(11111, 11131),
                            GetAvailablePort(30000, 30020),
                            listenOnAnyHostAddress: true);
#endif
                        builder.ConfigureApplicationParts(parts =>
                            parts
                                .AddApplicationPart(typeof(GameService).Assembly)
                                .AddApplicationPart(typeof(GameGrain).Assembly)
                                .AddApplicationPart(typeof(UserService).Assembly)
                                .AddApplicationPart(typeof(UserGrain).Assembly)
                                .WithReferences());
                    })
                    .RunConsoleAsync();
            }
            catch (Exception e)
            {
                Log.Fatal(e, "Host terminated unexpectedly");
                Log.CloseAndFlush();
                Environment.Exit(1);
                return Task.CompletedTask;
            }
        }

        private static void ConfigureContainer(ContainerBuilder builder)
        {
            // Grains
            builder.RegisterType<GameService>().AsSelf();
            builder.RegisterType<GameGrain>().AsSelf();
            builder.RegisterType<UserService>().AsSelf();
            builder.RegisterType<UserGrain>().AsSelf();

            // Repositories
            builder.RegisterType<MongoContext>().As<IMongoContext>().SingleInstance();
            builder.RegisterType<GameRepository>().As<IGameRepository>().SingleInstance();
            builder.RegisterType<UserRepository>().As<IUserRepository>().SingleInstance();
        }

        private static int GetAvailablePort(int start, int end)
        {
            for (var port = start; port < end; port++)
            {
                var listener = TcpListener.Create(port);
                listener.ExclusiveAddressUse = true;
                try
                {
                    listener.Start();
                    return port;
                }
                catch (SocketException) { }
                finally 
                {
                    listener.Stop();
                }
            }
            throw new InvalidOperationException();
        }
    }
}
