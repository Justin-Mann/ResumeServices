using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using System;

namespace ResumeCore.Helpers {
    public static class ClusterHelper {
        public static IConfiguration BootstrapKeyVault(this IConfigurationBuilder configBuilder) {
            var builtConfig = configBuilder.Build();

            var connectionString = AppEnvironment.Equals("Development", StringComparison.OrdinalIgnoreCase) ?
                                   "RunAs=Developer; DeveloperTool=VisualStudio" :
                                   "RunAs=App";

            var tokenProvider = new AzureServiceTokenProvider("RunAs=App");

            var kvClient = new KeyVaultClient((authority, resource, scope) => tokenProvider.KeyVaultTokenCallback(authority, resource, scope));

            configBuilder.AddAzureKeyVault(Environment.GetEnvironmentVariable("KeyVault_BaseUrl"), kvClient, new DefaultKeyVaultSecretManager());

            return configBuilder.Build();

        }

        public static string AppEnvironment => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "";

    }
}
