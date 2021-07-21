using Microsoft.Azure.Cosmos;
using ResumeCore.Entity.Base;
using ResumeCore.Interface;
using ResumeInfastructure.CosmosDbData.Interface;
using System;
using System.Linq;

namespace ResumeInfastructure.CosmosDbData.Repository {
    public class GenericRepository<T>: CosmosDbRepository<T>, IGenericRepository<T> where T : BaseEntity {
        /// <summary>
        ///     CosmosDB container name
        /// </summary>
        public override string ContainerName { get; } = typeof(T).ToString().Split(".").Last();

        /// <summary>
        ///     Generate Id.
        ///     e.g. "783dfe25-7ece-4f0b-885e-c0ea72135942"
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override string GenerateId(T entity) => $"{Guid.NewGuid()}";

        /// <summary>
        ///     Returns the value of the partition key
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public override PartitionKey ResolvePartitionKey(string entityId) => new(entityId.First().ToString());

        public GenericRepository(ICosmosDbContainerFactory factory) : base(factory) { }
    }
}
