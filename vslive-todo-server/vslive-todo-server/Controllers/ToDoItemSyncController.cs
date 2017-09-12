using System.Web.Http;
using Microsoft.Azure.Mobile.Server.Config;
using vslive_todo_server.Models;
using System.Threading.Tasks;
using vslive_todo_server.DataObjects;
using System.Data.Entity;
using System;
using Microsoft.Azure.NotificationHubs;
using System.Collections.Generic;

namespace vslive_todo_server.Controllers
{
    [MobileAppController]
    public class ToDoItemSyncController : ApiController
    {
        MobileServiceContext context;

        public ToDoItemSyncController() : base()
        {
            context = new MobileServiceContext();
        }

        [HttpPost]
        public async Task<CustomResponse> PostAsync([FromBody] TodoItem item)
        {
            using (DbContextTransaction transaction = context.Database.BeginTransaction())
            {
                try
                {
                    item.Id = Guid.NewGuid().ToString();
                    item.UpdatedAt = DateTimeOffset.Now;
                    item.CreatedAt = DateTimeOffset.Now;
                    context.TodoItems.Add(item);
                    await context.SaveChangesAsync();
                    transaction.Commit();

                    await PushToSyncAsync("ToDoItem", item.Id);

                } catch (Exception ex)
                {
                    transaction.Rollback();
                }

                return new CustomResponse { Status = 200 };
            }
        }

        private async Task PushToSyncAsync(string table, string id)
        {
            var appSettings = this.Configuration.GetMobileAppSettingsProvider().GetMobileAppSettings();
            var nhName = appSettings.NotificationHubName;
            var nhConnection = appSettings.Connections[MobileAppSettingsKeys.NotificationHubConnectionString].ConnectionString;

            // Create a new Notification Hub client
            var hub = NotificationHubClient.CreateClientFromConnectionString(nhConnection, nhName);

            // Create a template message
            var templateParams = new Dictionary<string, string>();
            templateParams["op"] = "sync";
            templateParams["table"] = table;
            templateParams["id"] = id;

            // Send the template message
            try
            {
                var result = await hub.SendTemplateNotificationAsync(templateParams);
                Configuration.Services.GetTraceWriter().Info(result.State.ToString());
            }
            catch (Exception ex)
            {
                Configuration.Services.GetTraceWriter().Error(ex.Message, null, "PushToSync Error");
            }
        }
    }

    public class CustomResponse
    {
        public int Status { get; set; }
    }
}
