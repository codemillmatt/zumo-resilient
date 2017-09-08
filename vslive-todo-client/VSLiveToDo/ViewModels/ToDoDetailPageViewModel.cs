using System;
using System.Threading.Tasks;
using MvvmHelpers;
using VSLiveToDo.Models;
using VSLiveToDo.Services;
using Xamarin.Forms;

namespace VSLiveToDo.ViewModels
{
    public class ToDoDetailPageViewModel : BaseViewModel
    {
        INavigation nav;

        public ToDoDetailPageViewModel(INavigation nav, ToDoItem todo = null)
        {
            if (todo != null)
            {
                Item = todo;
            }
            else
            {
                Item = new ToDoItem { Text = "New ToDo", Complete = false };
            }

            Title = Item.Text;

            this.nav = nav;
        }

        public ToDoItem Item { get; set; }

        Command saveCmd;
        public Command SaveCommand => saveCmd ?? (saveCmd = new Command(async () => await ExecuteSaveCmd()));

        private async Task ExecuteSaveCmd()
        {
            try
            {
                if (IsBusy)
                    return;
                IsBusy = true;

                var svc = new ZumoService();
                if (string.IsNullOrEmpty(Item.Id))
                    await svc.CreateToDo(Item);
                else
                    await svc.UpdateToDo(Item);

                MessagingCenter.Send(this, "refresh_list");
            }
            finally
            {
                IsBusy = false;
                await nav.PopAsync();
            }
        }

        Command cancelCmd;
        public Command CancelCommand => cancelCmd ?? (cancelCmd = new Command(async () => await nav.PopAsync()));
    }
}
