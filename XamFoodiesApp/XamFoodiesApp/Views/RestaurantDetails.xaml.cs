using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using XamFoodiesApp.ViewModels;

namespace XamFoodiesApp.Views
{
    public partial class RestaurantDetails : ContentPage
    {
        private RestaurantDetailsViewModel RestaurantViewModel;
        public RestaurantDetails(string RestuarantName)
        {
            NavigationPage.SetHasNavigationBar(this, false);
            RestaurantViewModel = new RestaurantDetailsViewModel(Navigation, this);
            BindingContext = RestaurantViewModel;
            InitializeComponent();
            headerLabel.Text = RestuarantName;
            RestaurantViewModel.ItemsList = new System.Collections.ObjectModel.ObservableCollection<Models.Item>() {
                new Models.Item() { Id=1, ItemName="Kadai Paneer", Price=200, Quantity=0, VegOrNonVeg="Veg"},
                new Models.Item() { Id = 2, ItemName = "Chicken Biryani", Price = 300, Quantity = 0, VegOrNonVeg = "Non Veg" },
                new Models.Item() { Id = 3, ItemName = "Butter Chicken", Price = 300, Quantity = 0, VegOrNonVeg = "Non Veg" },
                new Models.Item() { Id = 4, ItemName = "Butter Naan", Price = 10, Quantity = 0, VegOrNonVeg = "Veg" }
                };
            RestaurantViewModel.OrderedItems.Clear();
            var IsAdmin = DependencyService.Get<DependencyServices.ISettingService>().GetValueOrDefault<string>(Configuration.IS_ADMIN);
            if (IsAdmin.ToLower().Equals("true"))
                checkoutButton.IsVisible = false;
            else
                checkoutButton.IsVisible = true;
        }

        async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            var TotalAmount = 0;
            string items = "";
            var count = RestaurantViewModel.OrderedItems.Count;
            if (count > 0) {
                int i = 0;
                foreach (var item in RestaurantViewModel.OrderedItems)
                {
                    if(i==count-1)
                        items = items + "and "+item.Quantity + " X " + item.ItemName;
                    else
                        items = items + item.Quantity + " X " + item.ItemName + ", ";
                    TotalAmount = TotalAmount + (item.Quantity * item.Price);
                    i++;
                }
                await DisplayAlert("Order placed successfully", "You ordered : " + items + ". Your total bill is Rs. " + TotalAmount + ".", "OK");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Alert", "Please add atleast one item", "OK");
            }
            
        }

        void TapGestureRecognizer_Tapped(System.Object sender, System.EventArgs e)
        {
            var id = (int)((Button)sender).CommandParameter;
            RestaurantViewModel.OrderedItems.Add(RestaurantViewModel.ItemsList.Where(x => x.Id == id).FirstOrDefault());
        }

    }
}

