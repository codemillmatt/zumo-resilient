using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;
using Plugin.Connectivity;
using System.Linq;
using Plugin.Connectivity.Abstractions;

namespace VSLiveToDo.Core
{
    public class ZumoService
    {
        MobileServiceClient client;
        IMobileServiceSyncTable<ToDoItem> table;

        public ZumoService()
        {
            client = new MobileServiceClient("https://zumo-resilient.azurewebsites.net");
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

        public async Task SyncOfflineCache()
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                await Application.Current.MainPage.DisplayAlert("No Connection",
                        "Cannot connect to network.", "OK");

                return;
            }

            try
            {
                Settings.HasSyncStarted = true;

                await Initializer();

                if (CrossConnectivity.Current.ConnectionTypes.Any(ct => ct == ConnectionType.WiFi))
                {
                    await client.SyncContext.PushAsync();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Not so fast!",
                                                                    "Connection speed not fast enough to sync", "OK");

                    return;
                }

                await table.PullAsync("todo-incremental", table.CreateQuery());
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
            finally
            {
                Settings.HasSyncStarted = false;
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

                // First take the complted = always favor the server
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
                    // VERY IMPORTANGT!!
                    local.Version = server.Version;
                    await prError.UpdateOperationAsync(JObject.FromObject(local));

                    winnerName = "local";
                }

                await App.Current.MainPage.DisplayAlert("Conflict", $"We detected a conflict and chose the {winnerName}.", "OK");
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

        public async Task PurgeAll()
        {
            await this.Initializer();

            var query = table.CreateQuery();

            await table.PurgeAsync("todo-incremental", query, new System.Threading.CancellationToken());
        }
    }
}
