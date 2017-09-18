using Xamarin.Forms;
using VSLiveToDo.Services;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace VSLiveToDo
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

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

        protected async override void OnResume()
        {
            // Handle when your app resumes
            if (Settings.HasSyncStarted)
            {
                var zumoService = new ZumoService();

                var hasPending = await zumoService.HasPendingOperations();

                if (hasPending)
                    await zumoService.SyncOfflineCache();
            }
        }
    }
}
