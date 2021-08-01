using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ResumeCore.Helpers;

namespace InstitutionAPI {
    public class Program {
        public static void Main(string[] args) {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => {

                    webBuilder.ConfigureKestrel(serverOptions => {
                        serverOptions.AddServerHeader = false;
                    });

                    webBuilder.ConfigureAppConfiguration((hostingContext, configBuilder) => {
                        configBuilder
                            .AddJsonFile("appsettings.json", true, true)
                            .AddEnvironmentVariables()
                            .BootstrapKeyVault();

                        if ( args != null ) configBuilder.AddCommandLine(args);
                    });

                    webBuilder.ConfigureLogging(builder => {
                        builder.ClearProviders();
                        builder.AddConsole();
                        builder.AddDebug();
                    });

                    webBuilder.UseStartup<Startup>();
                });
    }
}

//TODO:: keyvault integration and either create a sp and use identity or use a cert.

