using Microsoft.Extensions.DependencyInjection;
using ResumeInfastructure.AppSettings;
using ResumeInfastructure.CosmosDbData.Interface;
using System.Collections.Generic;

namespace ResumeInfastructure.CosmosDbData.Extension {
    public static class IServiceCollectionExtensions {
        /// <summary>
        ///     Register a singleton instance of Cosmos Db Container Factory, which is a wrapper for the CosmosClient.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="endpointUrl"></param>
        /// <param name="primaryKey"></param>
        /// <param name="databaseName"></param>
        /// <param name="containers"></param>
        /// <returns></returns>
        public static IServiceCollection AddCosmosDb(this IServiceCollection services,
                                                     string endpointUrl,
                                                     string primaryKey,
                                                     string databaseName,
                                                     List<ContainerInfo> containers) {
            Microsoft.Azure.Cosmos.CosmosClient client = new(endpointUrl, primaryKey);
            var cosmosDbClientFactory = new CosmosDbContainerFactory(client, databaseName, containers);

            services.AddSingleton<ICosmosDbContainerFactory>(cosmosDbClientFactory);

            return services;
        }
    }
}
