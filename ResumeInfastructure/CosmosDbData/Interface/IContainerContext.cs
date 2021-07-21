using Microsoft.Azure.Cosmos;
using ResumeCore.Entity.Base;

namespace ResumeInfastructure.CosmosDbData.Interface {
    /// <summary>
    ///  Defines the container level context
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IContainerContext<T> where T : BaseEntity {
        string ContainerName { get; }
        string GenerateId(T entity);
        PartitionKey ResolvePartitionKey(string entityId);
    }
}
