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

            RefreshList();
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

        async Task RefreshList()
        {
            await ExecuteRefreshingCommand();
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
                    await service.SyncOfflineCache();

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
