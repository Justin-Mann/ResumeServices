using Microsoft.Azure.Cosmos;
using ResumeCore.Entity;
using ResumeCore.Interface;
using ResumeInfastructure.CosmosDbData.Interface;

namespace ResumeInfastructure.CosmosDbData.Repository {
    /// <summary>
    ///     Audit repository
    /// </summary>
    public class AuditRepository: CosmosDbRepository<Audit>, IAuditRepository {
        /// <summary>
        ///     Name of the cosmosDb container where entity records will reside.
        /// </summary>
        public override string ContainerName { get; } = "Audit";
        public override string GenerateId(Audit entity) => GenerateAuditId(entity);
        public override PartitionKey ResolvePartitionKey(string entityId) => ResolveAuditPartitionKey(entityId);

        public AuditRepository(ICosmosDbContainerFactory factory) : base(factory) { }
    }
}
