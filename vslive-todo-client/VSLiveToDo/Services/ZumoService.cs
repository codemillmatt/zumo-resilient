using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using VSLiveToDo.Models;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;
using VSLiveToDo.Abstractions;

using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;

namespace VSLiveToDo.Services
{
    public class ZumoService
    {
        MobileServiceClient client;
        IMobileServiceSyncTable<ToDoItem> table;

        bool haveWiFi;
        public ZumoService()
        {
            client = new MobileServiceClient("https://vslive-chicago-zumo.azurewebsites.net");

            haveWiFi = CrossConnectivity.Current.ConnectionTypes.Any(ct => ct == ConnectionType.WiFi);
        }

        async Task Initializer()
        {
            if (client.SyncContext.IsInitialized)
                return;

            var store = new MobileServiceSQLiteStore("todos.db");

            store.DefineTable<ToDoItem>();

            await client.SyncContext.InitializeAsync(store);
            table = client.GetSyncTable<ToDoItem>();
        }

        public async Task<bool> HasPendingOperations()
        {
            await Initializer();

            return client.SyncContext.PendingOperations > 0;
        }

        public async Task SyncOfflineCache()
        {
            try
            {
                await Initializer();

                if (CrossConnectivity.Current.IsConnected)
                {
                    if (client.SyncContext.PendingOperations <= 8 || haveWiFi)
                    {
                        await client.SyncContext.PushAsync();
                        await table.PullAsync("todo-incremental", table.CreateQuery());
                    }
                }
            }
            catch (MobileServicePreconditionFailedException<ToDoItem> precondEx)
            {
                // happens for online only
            }
            catch (MobileServicePushFailedException ex)
            {
                if (ex.PushResult != null)
                {
                    await ResolveConflicts(ex.PushResult);
                }
            }
        }

        async Task ResolveConflicts(MobileServicePushCompletionResult result)
        {
            foreach (var prError in result.Errors)
            {
                bool serverWins = false;
                bool localWins = false;

                var server = prError.Result.ToObject<ToDoItem>();
                var local = prError.Item.ToObject<ToDoItem>();

                // First take the completed = always favor the server
                if (server.Complete)
                {
                    serverWins = true;
                }
                else if (local.Complete)
                {
                    localWins = true;
                }

                // Longer description
                if (!serverWins && !localWins)
                {
                    if (server.Notes.Length >= local.Notes.Length)
                        serverWins = true;
                    else
                        localWins = true;
                }

                var winnerName = "";
                if (serverWins)
                {
                    await prError.CancelAndUpdateItemAsync(prError.Result);

                    winnerName = "server";
                }
                else
                {
                    // VERY IMPORTANT!!
                    local.Version = server.Version;
                    await prError.UpdateOperationAsync(JObject.FromObject(local));

                    winnerName = "local";
                }

                await Application.Current.MainPage.DisplayAlert("Conflict", $"We detected a conflict and chose the {winnerName}.", "OK");
            }

            // This is so we can get a pull of the data back out
            await SyncOfflineCache();
        }

        public async Task<List<ToDoItem>> GetAllToDoItems()
        {
            await this.Initializer();

            return await table.ToListAsync();
        }

        public async Task CreateToDo(ToDoItem item)
        {
            await this.Initializer();

            await table.InsertAsync(item);
        }

        public async Task UpdateToDo(ToDoItem item)
        {
            await this.Initializer();

            await table.UpdateAsync(item);
        }

        public async Task DeleteToDo(ToDoItem item)
        {
            await this.Initializer();

            await table.DeleteAsync(item);
        }

        public async Task<List<ToDoItem>> PerformMassInsert()
        {
            await this.Initializer();

            var theItems = new List<ToDoItem>();
            for (int i = 0; i < 20; i++)
            {
                var item = new ToDoItem { Complete = false, Text = $"Mass insert #: {i}", Notes = "That's a lot!" };
                theItems.Add(item);

                await table.InsertAsync(item);
            }

            return theItems;
        }

        public async Task RegisterForPushNotifications()
        {
            var platform = DependencyService.Get<IPlatformProvider>();

            await platform.RegisterForPushNotifications(client);
        }
    }
}
