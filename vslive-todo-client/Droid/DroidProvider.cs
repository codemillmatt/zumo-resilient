using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using VSLiveToDo.Abstractions;
using Xamarin.Forms;
using VSLiveToDo.Droid;
using Android.Content;
using Gcm.Client;

[assembly: Dependency(typeof(DroidProvider))]
namespace VSLiveToDo.Droid
{
    public class DroidProvider : IPlatformProvider
    {
        public async Task RegisterForPushNotifications(MobileServiceClient client)
        {
            var registrationId = GcmClient.GetRegistrationId(RootView);

            var azurePush = client.GetPush();
            await azurePush.RegisterAsync(registrationId);
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
