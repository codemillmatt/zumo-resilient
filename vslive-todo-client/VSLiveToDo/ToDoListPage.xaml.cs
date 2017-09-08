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
    }
}
