using Microsoft.Azure.Cosmos;

namespace ResumeInfastructure.CosmosDbData.Interface {
    public interface ICosmosDbContainer {
        /// <summary>
        ///     Azure Cosmos DB Container
        /// </summary>
        Container _container { get; }
    }
}
