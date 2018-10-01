using System;
using System.Collections.Generic;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

using Xamarin.Forms;

namespace VSLiveToDo.Core
{
    public partial class ToDoDetailPage : ContentPage
    {

        public ToDoDetailPage(ToDoItem todo)
        {
            InitializeComponent();

            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);

            BindingContext = new ToDoDetailPageViewModel(Navigation, todo);
        }

        public ToDoDetailPage()
        {
            InitializeComponent();

            BindingContext = new ToDoDetailPageViewModel(Navigation);
        }
    }
}
