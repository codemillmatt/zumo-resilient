using System;
using System.Threading.Tasks;
using MvvmHelpers;
using Xamarin.Forms;
using System.Linq;

namespace VSLiveToDo.Core
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

        ObservableRangeCollection<ToDoItem> items = new ObservableRangeCollection<ToDoItem>();
        public ObservableRangeCollection<ToDoItem> Items
        {
            get { return items; }
            set
            {
                SetProperty(ref items, value, nameof(Items));
            }
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

        Command<ToDoItem> deleteCommand;
        public Command<ToDoItem> DeleteCommand => deleteCommand ?? (deleteCommand =
                                                          new Command<ToDoItem>(async (todo) =>
                                                          {
                                                              var zumo = new ZumoService();
                                                              await zumo.DeleteToDo(todo);

                                                              Items.Remove(todo);
                                                          }));

        Command purgeCommand;
        public Command PurgeCommand => purgeCommand ?? (purgeCommand =
                                                        new Command(async () =>
                                                        {
                                                            var zumo = new ZumoService();
                                                            await zumo.PurgeAll();
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
            finally
            {
                IsBusy = false;
            }
        }
    }
}
