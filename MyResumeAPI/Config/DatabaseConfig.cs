using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ResumeCore.Interface;
using ResumeInfastructure.AppSettings;
using ResumeInfastructure.CosmosDbData.Extension;
using ResumeInfastructure.CosmosDbData.Repository;

namespace MyResumeAPI.Config {
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
            var t = configuration.GetValue<string>("ResumeServices:ConnectionStrings:CosmosDb:PrimaryKey");
            // Bind database-related bindings
            CosmosDbSettings cosmosDbConfig = configuration.GetSection("CosmosDB").Get<CosmosDbSettings>();
            // register CosmosDB client and data repositories
            services.AddCosmosDb(cosmosDbConfig.EndpointUrl,
                                 string.IsNullOrEmpty(cosmosDbConfig.PrimaryKey) ? configuration["ResumeServices:ConnectionStrings:CosmosDb:PrimaryKey"] : cosmosDbConfig.PrimaryKey,
                                 cosmosDbConfig.DatabaseName,
                                 cosmosDbConfig.Containers);

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IAuditRepository, AuditRepository>();
        }
    }
}
