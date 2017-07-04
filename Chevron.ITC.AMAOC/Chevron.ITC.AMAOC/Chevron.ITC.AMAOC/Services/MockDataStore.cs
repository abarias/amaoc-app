using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Chevron.ITC.AMAOC.Models;
using Chevron.ITC.AMAOC.DataObjects;

namespace Chevron.ITC.AMAOC.Services
{
    public class MockDataStore : IDataStore<Event>
    {
        bool isInitialized;
        List<Event> items;

        public async Task<bool> AddItemAsync(Event item)
        {
            await InitializeAsync();

            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Event item)
        {
            await InitializeAsync();

            var _item = items.Where((Event arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(_item);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(Event item)
        {
            await InitializeAsync();

            var _item = items.Where((Event arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(_item);

            return await Task.FromResult(true);
        }

        public async Task<Event> GetItemAsync(string id)
        {
            await InitializeAsync();

            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Event>> GetItemsAsync(bool forceRefresh = false)
        {
            await InitializeAsync();

            return await Task.FromResult(items);
        }

        public Task<bool> PullLatestAsync()
        {
            return Task.FromResult(true);
        }


        public Task<bool> SyncAsync()
        {
            return Task.FromResult(true);
        }

        public async Task InitializeAsync()
        {
            if (isInitialized)
                return;

            items = new List<Event>();
            var _items = new List<Event>
            {
                new Event { Id = Guid.NewGuid().ToString(), Name = "Buy some cat food", Abstract="The cats are hungry"},
                new Event { Id = Guid.NewGuid().ToString(), Name = "Learn F#", Abstract="Seems like a functional idea"},
                new Event { Id = Guid.NewGuid().ToString(), Name = "Learn to play guitar", Abstract="Noted"},
                new Event { Id = Guid.NewGuid().ToString(), Name = "Buy some new candles", Abstract="Pine and cranberry for that winter feel"},
                new Event { Id = Guid.NewGuid().ToString(), Name = "Complete holiday shopping", Abstract="Keep it a secret!"},
                new Event { Id = Guid.NewGuid().ToString(), Name = "Finish a todo list", Abstract="Done"},
            };

            foreach (Event item in _items)
            {
                items.Add(item);
            }

            isInitialized = true;
        }
    }
}
