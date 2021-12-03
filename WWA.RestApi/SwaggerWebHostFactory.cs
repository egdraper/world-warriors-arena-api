using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace WWA.RestApi
{
    public class SwaggerWebHostFactory
    {
        public static IWebHost CreateWebHost()
        {
            return WebHost.CreateDefaultBuilder()
                .ConfigureAppConfiguration((context, config) =>
                {
                    config
                        .SetBasePath(context.HostingEnvironment.ContentRootPath)
                        .AddJsonFile("config.json", true)
                        .AddJsonFile($@"{context.HostingEnvironment.ContentRootPath}\..\config.json", optional: true)
                        .AddJsonFile(Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".wwa", "config.json"), true)
                        .AddEnvironmentVariables();
                })
                .ConfigureServices(s => s.AddAutofac())
                .UseStartup<Startup>()
                .Build();
        }
    }
}
