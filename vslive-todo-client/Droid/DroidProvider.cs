using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using VSLiveToDo.Abstractions;
using Xamarin.Forms;
using VSLiveToDo.Droid;
using Android.Content;
using Gcm.Client;
using VSLiveToDo.Push;
using System.Net.Http;

[assembly: Dependency(typeof(DroidProvider))]
namespace VSLiveToDo.Droid
{
    public class DroidProvider : IPlatformProvider
    {
        public async Task RegisterForPushNotifications(MobileServiceClient client)
        {
            var registrationId = GcmClient.GetRegistrationId(RootView);

            var azurePush = client.GetPush();

            var installation = new DeviceInstallation
            {
                InstallationId = client.InstallationId,
                Platform = "gcm",
                PushChannel = registrationId
            };

            installation.Tags.Add("silent-push");

            var silentTemplate = new PushTemplate
            {
                Body = @"{""data"":{""op"":""$(op)"",""table"":""$(table)"",""id"":""$(id)""}}"
            };
            installation.Templates.Add("silent", silentTemplate);

            var response = await client.InvokeApiAsync<DeviceInstallation, DeviceInstallation>(
                $"/push/installations/{client.InstallationId}",
                installation,
                HttpMethod.Put,
                new System.Collections.Generic.Dictionary<string, string>());
        }

        public Context RootView { private set; get; }

        public void Init(Context context)
        {
            RootView = context;

            GcmClient.CheckDevice(RootView);
            GcmClient.CheckManifest(RootView);

            GcmClient.Register(RootView, GcmHandler.SenderId);
        }
    }
}
