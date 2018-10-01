using System;
using Xamarin.Forms;
using Plugin.Settings;

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
            // Handle when your app starts
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
