using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chevron.ITC.AMAOC.Abstractions
{
    public interface IBaseStore<T>
    {
        Task InitializeStore();
        Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false);
        Task<T> GetItemAsync(string id);
        Task<bool> AddItemAsync(T item);
        Task<bool> UpdateItemAsync(T item);
        Task<bool> DeleteItemAsync(T item);
        Task<bool> SyncAsync();

        void DropTable();

        Task<bool> PullLatestAsync();

        string Identifier { get; }
        
    }
}
