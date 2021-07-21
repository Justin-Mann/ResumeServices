using ResumeCore.Entity.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ResumeCore.Interface {
    public interface IRepository<T> where T : BaseEntity {
        /// <summary>
        ///     Get items given a string SQL query directly.
        ///     Likely in production, you may want to use alternatives like Parameterized Query or LINQ to avoid SQL Injection and avoid having to work with strings directly.
        ///     This is kept here for demonstration purpose.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> GetItemsAsync(string query);

        Task<T> GetItemAsync(string id);
        Task<string> AddItemAsync(T item);
        Task<bool> UpdateItemAsync(string id, T item);
        Task<bool> DeleteItemAsync(string id);
    }
}
