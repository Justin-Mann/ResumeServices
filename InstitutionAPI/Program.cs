using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace InstitutionAPI {
    public class Program {
        public static void Main(string[] args) {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) => {
                    var builtConfig = config.Build();

                    if ( string.IsNullOrEmpty(builtConfig["KeyVaultName"]) )
                        throw new Exception("Configure the KeyVaultName by using an Environment Variable");

                    var azureServiceTokenProvider = new AzureServiceTokenProvider();
                    var keyVaultClient = new KeyVaultClient(
                        new KeyVaultClient.AuthenticationCallback(
                            azureServiceTokenProvider.KeyVaultTokenCallback));

                    config.AddAzureKeyVault(
                        $"https://{builtConfig["KeyVaultName"]}.vault.azure.net/",
                        keyVaultClient,
                        new DefaultKeyVaultSecretManager());
                })
                .ConfigureWebHostDefaults(webBuilder => {

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
    }
}

//TODO:: keyvault integration and either create a sp and use identity or use a cert.

