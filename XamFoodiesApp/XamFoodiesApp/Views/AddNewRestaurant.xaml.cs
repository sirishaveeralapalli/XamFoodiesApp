using System;
using System.Collections.Generic;

using Xamarin.Forms;
using XamFoodiesApp.ViewModels;

namespace XamFoodiesApp.Views
{
    public partial class AddNewRestaurant : ContentPage
    {
        private AddNewRestViewModel addRestaurantViewModel;
        public int restaurantId;
        public AddNewRestaurant(Models.Restaurant restaurant)
        {
            NavigationPage.SetHasNavigationBar(this, false);
            addRestaurantViewModel = new AddNewRestViewModel(Navigation, this);
            BindingContext = addRestaurantViewModel;
            InitializeComponent();
            setValues(restaurant);
            restaurantId = restaurant!=null? restaurant.Id:0;
            //mainLayout.Padding = new Thickness(0, 80, 0, 0);
        }

        private void setValues(Models.Restaurant restaurant)
        {
            if (restaurant != null)
            {
                addRestaurantViewModel.DisplayName = restaurant.DisplayName;
                addRestaurantViewModel.AddressLine1 = restaurant.Address;
                addRestaurantViewModel.PriceForTwo = restaurant.PriceForTwo;
            }
        }
    }
}

