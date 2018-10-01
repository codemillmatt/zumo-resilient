using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace VSLiveToDo.Core
{
    public partial class ToDoListPage : ContentPage
    {
        public ToDoListPage()
        {
            InitializeComponent();

            Title = "VS Live";
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);

            BindingContext = new ToDoListPageViewModel(this.Navigation);
        }

        public void Delete_Clicked(object sender, EventArgs e)
        {
            var mi = (MenuItem)sender;
            var vm = (ToDoListPageViewModel)BindingContext;

            vm.DeleteCommand.Execute(mi.CommandParameter);
        }
    }
}
