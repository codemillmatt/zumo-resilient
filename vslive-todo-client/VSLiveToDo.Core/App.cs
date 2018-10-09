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


    }
}
