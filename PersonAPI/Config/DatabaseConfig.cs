using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ResumeCore.Interface;
using ResumeInfastructure.AppSettings;
using ResumeInfastructure.CosmosDbData.Extension;
using ResumeInfastructure.CosmosDbData.Repository;

namespace PersonAPI.Config {
    /// <summary>
    ///     Database related configurations
    /// </summary>
    public static class DatabaseConfig {
        /// <summary>
        ///     Setup Cosmos DB
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void SetupCosmosDb(this IServiceCollection services, IConfiguration configuration) {
            // Bind database-related bindings
            CosmosDbSettings cosmosDbConfig = configuration.GetSection("ConnectionStrings:CosmosDB").Get<CosmosDbSettings>();
            // register CosmosDB client and data repositories
            services.AddCosmosDb(cosmosDbConfig.EndpointUrl,
                                 cosmosDbConfig.PrimaryKey ?? configuration.GetValue<string>("ResumeServices:ConnectionStrings:CosmosDb:PrimaryKey"),
                                 cosmosDbConfig.DatabaseName,
                                 cosmosDbConfig.Containers);

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IAuditRepository, AuditRepository>();
        }
    }
}
