using System;
using System.Collections.Generic;

using Xamarin.Forms;

using VSLiveToDo.Models;
using VSLiveToDo.ViewModels;

namespace VSLiveToDo
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
