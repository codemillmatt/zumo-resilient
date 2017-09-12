using System;
using System.Collections.ObjectModel;
using MvvmHelpers;
using VSLiveToDo.Models;
using System.Threading.Tasks;
using System.Diagnostics;
using VSLiveToDo.Services;
using Xamarin.Forms;

namespace VSLiveToDo.ViewModels
{
    public class ToDoListPageViewModel : BaseViewModel
    {
        INavigation navigation;
        public ToDoListPageViewModel(INavigation nav)
        {
            Title = "To Do Items";

            navigation = nav;

            InitialRefreshList();

            var service = new ZumoService();
            Task.Run(async () =>
            {
                await service.RegisterForPushNotifications();
            });

            MessagingCenter.Subscribe<PushToSync>(this, "ItemsChanged", async (obj) =>
            {
                await ExecuteRefreshingCommand(true);
            });
        }

        bool isRefreshing;
        public bool IsRefreshing
        {
            get
            {
                return isRefreshing;
            }
            set
            {
                SetProperty(ref isRefreshing, value, nameof(IsRefreshing));
            }
        }

        ObservableCollection<ToDoItem> items = new ObservableCollection<ToDoItem>();
        public ObservableCollection<ToDoItem> Items
        {
            get { return items; }
            set { SetProperty(ref items, value, nameof(Items)); }
        }

        ToDoItem selectedItem;
        public ToDoItem SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                SetProperty(ref selectedItem, value, nameof(SelectedItem));

                if (selectedItem != null)
                {
                    navigation.PushAsync(new ToDoDetailPage(selectedItem));
                    SelectedItem = null;
                }
            }
        }

        Command refreshCommand;
        public Command RefreshCommand => refreshCommand ?? (refreshCommand =
                                                            new Command(async () => await ExecuteRefreshingCommand(true)));

        Command addNewCommand;
        public Command AddNewCommand => addNewCommand ?? (addNewCommand =
                                                          new Command(async () =>
                                                          {
                                                              if (IsBusy)
                                                                  return;
                                                              IsBusy = true;

                                                              try
                                                              {
                                                                  await navigation.PushAsync(new ToDoDetailPage());
                                                              }
                                                              finally
                                                              {
                                                                  IsBusy = false;
                                                              }
                                                          }));


        async Task InitialRefreshList()
        {
            await ExecuteRefreshingCommand();

            MessagingCenter.Subscribe<ToDoDetailPageViewModel>(this, "refresh_list", async (obj) => await ExecuteRefreshingCommand());
        }

        async Task ExecuteRefreshingCommand(bool pullFromCloud = false)
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                var service = new ZumoService();

                if (pullFromCloud)
                {
                    IsRefreshing = true;
                    await service.SyncOfflineCache();
                    IsRefreshing = false;
                }

                var results = await service.GetAllToDoItems();

                Items.Clear();
                foreach (var item in results)
                {
                    Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"The error was: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
