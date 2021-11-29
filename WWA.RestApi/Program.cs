using Autofac.Extensions.DependencyInjection;
using Common.Logging;
using Common.Logging.Serilog;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.IO;
using System.Threading.Tasks;

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
                .Enrich.FromLogContext()
                .WriteTo.Async(a => a.Console(
                    outputTemplate: "[{Timestamp:s} {Level:u3}] {Properties} {Message:lj}{NewLine}{Exception}", theme: ConsoleTheme.None))
                .CreateLogger();
            LogManager.Adapter = new SerilogFactoryAdapter(Log.Logger);
        }
        private static async Task Main(string[] args)
        {
            try
            {
                await CreateWebHostBuilder(args).Build().RunAsync();
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

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            Log.Information("Starting service...");
            return WebHost
                .CreateDefaultBuilder(args)
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
                .ConfigureServices((config, services) =>
                {
                    services.AddAutofac();
                })
                .UseSerilog()
                .UseStartup<Startup>();
        }
    }
}
