using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RoboWhatsApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Threading.Tasks;
using RoboWhatsApp.Settings;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace RoboWhatsApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).ConfigureAppConfiguration((hostContext, config) =>
            {
                config.AddEnvironmentVariables();

                if (hostContext.HostingEnvironment.IsDevelopment())
                    config.AddUserSecrets<Program>();

            })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();

                    var environmentConf = hostContext.Configuration.GetSection("Environment");
                    var extConn = hostContext.Configuration.GetSection("ExternalConnection");
                    var applic = hostContext.Configuration.GetSection("Application");

                    services.Configure<Settings.Environment>(environmentConf);
                    services.Configure<Settings.ExternalConnection>(extConn);
                    services.Configure<Settings.Application>(applic);

                    services.AddSingleton<ExternalConnection>();
                    


                });
    }
}
