using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace VSLiveToDo
{
    public partial class ToDoListPage : ContentPage
    {
        public ToDoListPage()
        {
            InitializeComponent();

            BindingContext = new ViewModels.ToDoListPageViewModel(this.Navigation);
        }

        public void Delete_Clicked(object sender, EventArgs e)
        {
            var mi = (MenuItem)sender;
            var vm = (ViewModels.ToDoListPageViewModel)BindingContext;

            vm.DeleteCommand.Execute(mi.CommandParameter);
        }
    }
}
