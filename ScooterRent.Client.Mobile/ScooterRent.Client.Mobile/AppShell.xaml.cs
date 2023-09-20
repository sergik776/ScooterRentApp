using App2.ViewModels;
using App2.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace App2
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
