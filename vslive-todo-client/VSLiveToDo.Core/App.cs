using System;
using Xamarin.Forms;
using Plugin.Settings;

using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Push;
using System.Collections.Generic;

namespace VSLiveToDo.Core
{
    public class App : Application
    {
        public App()
        {
            MainPage = new NavigationPage(new ToDoListPage());
        }

        protected override void OnStart()
        {
            if (!AppCenter.Configured)
            {
                Push.PushNotificationReceived += async (sender, e) =>
                {
                    var zumoService = new ZumoService();
                    await zumoService.SyncOfflineCache();

                    // Add the notification message and title to the message
                    //var summary = $"Push notification received:" +
                    //                    $"\n\tNotification title: {e.Title}" +
                    //                    $"\n\tMessage: {e.Message}";

                    //// If there is custom data associated with the notification,
                    //// print the entries
                    //if (e.CustomData != null)
                    //{
                    //    summary += "\n\tCustom data:\n";
                    //    foreach (var key in e.CustomData.Keys)
                    //    {
                    //        summary += $"\t\t{key} : {e.CustomData[key]}\n";

                    //        Analytics.TrackEvent("Push", new Dictionary<string, string> { { "push-key", $"{key}" } });
                    //    }
                    //}

                    //// Send the notification summary to debug output
                    //System.Diagnostics.Debug.WriteLine(summary);
                };
            }


            AppCenter.Start("ios=dcfb280a-b080-474b-8c3b-38baa5739d0f;"
                            + "android=0418d688-810c-47f6-b532-7dbab8cdab5c",
                            typeof(Analytics),
                            typeof(Crashes),
                            typeof(Push));

            Push.SetEnabledAsync(true);

            OnResume();
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        //protected async override void OnResume()
        //{
        //    // Handle when your app resumes
        //    //if (Plugin.HasSyncStarted)
        //    //{
        //    //    var zumoService = new ZumoService();

        //    //    var hasPending = await zumoService.HasPendingOperations();

        //    //    if (hasPending)
        //    //        await zumoService.SyncOfflineCache();
        //    //}
        //}
    }
}
