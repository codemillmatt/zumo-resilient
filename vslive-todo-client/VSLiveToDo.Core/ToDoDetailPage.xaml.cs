using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace VSLiveToDo.Core
{
    public partial class ToDoDetailPage : ContentPage
    {

        public ToDoDetailPage(ToDoItem todo)
        {
            InitializeComponent();

            BindingContext = new ToDoDetailPageViewModel(Navigation, todo);
        }

        public ToDoDetailPage()
        {
            InitializeComponent();

            BindingContext = new ToDoDetailPageViewModel(Navigation);
        }
    }
}
