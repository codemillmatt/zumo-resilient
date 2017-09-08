using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;

using VSLiveToDo.Models;

namespace VSLiveToDo.Services
{
    public class ZumoService
    {
        MobileServiceClient client;

        public ZumoService()
        {
            client = new MobileServiceClient("https://vslive-chicago-zumo.azurewebsites.net");
        }

        async Task Initializer()
        {
            if (client.SyncContext.IsInitialized)
                return;

            var store = new MobileServiceSQLiteStore("todos.db");

            store.DefineTable<ToDoItem>();

            await client.SyncContext.InitializeAsync(store);
        }

        public async Task<bool> HasPendingOperations()
        {
            await Initializer();

            return client.SyncContext.PendingOperations > 0;
        }

        public async Task SyncOfflineCache(bool throwException = false)
        {
            try
            {
                Settings.HasSyncStarted = true;

                // Only here to simulate something bad happening :)
                if (throwException)
                    throw new Exception();

                await Initializer();

                await client.SyncContext.PushAsync();

                var table = client.GetSyncTable<ToDoItem>();
                await table.PullAsync("todo-incremental", table.CreateQuery());
            }
            finally
            {
                Settings.HasSyncStarted = false;
            }

        }

        public async Task<List<ToDoItem>> GetAllToDoItems()
        {
            await this.Initializer();

            var table = client.GetSyncTable<ToDoItem>();

            return await table.ToListAsync();
        }
    }
}
