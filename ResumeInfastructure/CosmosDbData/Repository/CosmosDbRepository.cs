using Microsoft.Azure.Cosmos;
using ResumeCore.Entity;
using ResumeCore.Entity.Base;
using ResumeCore.Interface;
using ResumeInfastructure.CosmosDbData.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResumeInfastructure.CosmosDbData.Repository {
    public abstract class CosmosDbRepository<T>: IRepository<T>, IContainerContext<T> where T : BaseEntity {
        /// <summary>
        ///     Name of the CosmosDB container
        /// </summary>
        public abstract string ContainerName { get; }

        /// <summary>
        ///     Generate id
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public abstract string GenerateId(T entity);

        /// <summary>
        ///     Audit container that will store audit log for all entities.
        /// </summary>
        private readonly Container _auditContainer;

        /// <summary>
        ///     Generate id for the audit record.
        ///     All entities will share the same audit container,
        ///     so we can define this method here with virtual default implementation.
        ///     Audit records for different entities will use different partition key values,
        ///     so we are not limited to the 20G per logical partition storage limit.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual string GenerateAuditId(Audit entity) => $"{entity.EntityId}:{Guid.NewGuid()}";

        /// <summary>
        ///     Resolve the partition key for the audit record.
        ///     All entities will share the same audit container,
        ///     so we can define this method here with virtual default implementation.
        ///     Audit records for different entities will use different partition key values,
        ///     so we are not limited to the 20G per logical partition storage limit.
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public virtual PartitionKey ResolveAuditPartitionKey(string entityId) => new PartitionKey($"{entityId}");

        /// <summary>
        ///     Resolve the partition key
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public abstract PartitionKey ResolvePartitionKey(string entityId);

        private readonly ICosmosDbContainerFactory _cosmosDbContainerFactory;
        private readonly Container _container;

        public CosmosDbRepository(ICosmosDbContainerFactory cosmosDbContainerFactory) {
            _cosmosDbContainerFactory = cosmosDbContainerFactory ?? throw new ArgumentNullException(nameof(ICosmosDbContainerFactory));
            _container = _cosmosDbContainerFactory.GetContainer(ContainerName)._container;
            _auditContainer = _cosmosDbContainerFactory.GetContainer("Audit")._container;
        }

        public async Task<string> AddItemAsync(T item) {
            item.Id = item.Id ?? GenerateId(item);
            item.PartitionKey = item.Id.First().ToString();
            await _container.CreateItemAsync<T>(item, ResolvePartitionKey(item.Id));
            await Audit(item); 
            return item?.Id;
        }

        public async Task<bool> DeleteItemAsync(string id) {
            await Audit(await GetItemAsync(id));
            await this._container.DeleteItemAsync<T>(id, ResolvePartitionKey(id));
            return true;
        }

        public async Task<T> GetItemAsync(string id) {
            try {
                ItemResponse<T> response = await _container.ReadItemAsync<T>(id, ResolvePartitionKey(id));
                return response.Resource;
            } catch ( CosmosException ex ) when ( ex.StatusCode == System.Net.HttpStatusCode.NotFound ) {
                return null;
            }
        }

        // Search data using SQL query string
        // This shows how to use SQL string to read data from Cosmos DB for demonstration purpose.
        // For production, try to use safer alternatives like Parameterized Query and LINQ if possible.
        // Using string can expose SQL Injection vulnerability, e.g. select * from c where c.id=1 OR 1=1. 
        // String can also be hard to work with due to special characters and spaces when advanced querying like search and pagination is required.
        public async Task<IEnumerable<T>> GetItemsAsync(string queryString) {
            var query = _container.GetItemQueryIterator<T>(new QueryDefinition(queryString));
            List<T> results = new();
            while ( query.HasMoreResults ) {
                var response = await query.ReadNextAsync();
                results.AddRange(response.ToList());
            }

            return results;
        }

        public async Task<bool> UpdateItemAsync(string id, T item) {
            await this._container.UpsertItemAsync<T>(item, ResolvePartitionKey(id));
            await Audit(item);
            return true;
        }

        /// <summary>
        ///     Audit a item by adding it to the audit container
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private async Task Audit(T item) {
            Audit auditItem = new Audit(item.GetType().Name,
                                        item.Id,
                                        Newtonsoft.Json.JsonConvert.SerializeObject(item));
            auditItem.Id = GenerateAuditId(auditItem);
            auditItem.PartitionKey = auditItem.Id.First().ToString();
            await _auditContainer.CreateItemAsync<Audit>(auditItem, ResolvePartitionKey(auditItem.EntityId));
        }
    }
}
