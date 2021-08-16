using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace MyResumeAPI {
    public class Program {
        public static void Main(string[] args) {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => {

                    webBuilder.ConfigureAppConfiguration((context, config) =>
                    {
                        if (context.HostingEnvironment.IsProduction())
                        {
                            var builtConfig = config.Build();
                            var secretClient = new SecretClient(
                                new Uri($"https://{builtConfig["KeyVaultName"]}.vault.azure.net/"),
                                new DefaultAzureCredential());
                            config.AddAzureKeyVault(secretClient, new KeyVaultSecretManager());
                        }
                    });

                    webBuilder.ConfigureKestrel(serverOptions => {
                        serverOptions.AddServerHeader = false;
                    });

                    webBuilder.ConfigureLogging(builder => {
                        builder.ClearProviders();
                        builder.AddConsole();
                        builder.AddDebug();
                    });

                    webBuilder.UseStartup<Startup>();
                });
        public static string AppEnvironment => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "";
    }
}
