using System;
using System.Collections.Generic;

using Xamarin.Forms;
using XamFoodiesApp.ViewModels;

namespace XamFoodiesApp.Views
{
    public partial class LoginPage : ContentPage
    {
        private LoginViewModel loginViewModel;
        public LoginPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            loginViewModel = new LoginViewModel(Navigation, this);
            BindingContext = loginViewModel;
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            loginViewModel.UserName = string.Empty;
            loginViewModel.Password = string.Empty;
        }
            //Login
            async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            if (string.IsNullOrEmpty(username.Text.ToString()))
            {
                DisplayAlert("Alert", "Please enter username", "OK");
                return;
            }
            if (string.IsNullOrEmpty(password.Text.ToString()))
            {
                DisplayAlert("Alert", "Please enter password", "OK");
                return;
            }
            if (username.Text.Equals("admin") && password.Text.Equals("admin"))
            {
                await Navigation.PushAsync(new RestaurantListView());
            }
            else
            {
                await DisplayAlert("Alert", "Please check your credentials", "OK");
                return;

            }
        }
    }
}

