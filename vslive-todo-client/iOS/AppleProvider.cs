using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using VSLiveToDo.Abstractions;
using VSLiveToDo.Push;
using System.Collections.Generic;
using System.Net.Http;

namespace VSLiveToDo.iOS
{
    public class AppleProvider : IPlatformProvider
    {
        public AppleProvider()
        {
        }

        public async Task RegisterForPushNotifications(MobileServiceClient client)
        {
            if (AppDelegate.PushDeviceToken != null)
            {
                try
                {
                    var registrationId = AppDelegate.PushDeviceToken.Description
                        .Trim('<', '>').Replace(" ", string.Empty).ToUpperInvariant();
                    var installation = new DeviceInstallation
                    {
                        InstallationId = client.InstallationId,
                        Platform = "apns",
                        PushChannel = registrationId
                    };
                    // Set up tags to request
                    installation.Tags.Add("silent-push");
                    // Set up templates to request
                    PushTemplate silentTemplate = new PushTemplate
                    {
                        Body = @"{""data"":{""op"":""$(op)"",""table"":""$(table)"",""id"":""$(id)""}}"
                    };

                    installation.Templates.Add("genericTemplate", silentTemplate);

                    // Register with NH
                    var response = await client.InvokeApiAsync<DeviceInstallation, DeviceInstallation>(
                        $"/push/installations/{client.InstallationId}",
                        installation,
                        HttpMethod.Put,
                        new Dictionary<string, string>());
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Fail($"[iOSPlatformProvider]: Could not register with NH: {ex.Message}");
                }
            }
        }
    }
}