using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ResumeInfastructure.CosmosDbData.Interface;

namespace ResumeInfastructure.CosmosDbData.Extension {
    public static partial class IApplicationBuilderExtensions {
        /// <summary>
        ///     Ensure Cosmos DB is created
        /// </summary>
        /// <param name="builder"></param>
        public static void EnsureCosmosDbIsCreated(this IApplicationBuilder builder) {
            using IServiceScope serviceScope = builder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            ICosmosDbContainerFactory factory = serviceScope.ServiceProvider.GetService<ICosmosDbContainerFactory>();

            factory.EnsureDbSetupAsync().Wait();
        }
    }
}
