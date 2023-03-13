using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamFoodiesApp.ViewModels;

namespace XamFoodiesApp.Views
{
    public partial class RestaurantListView : ContentPage
    {
        private RestaurantListViewModel RestaurantViewModel;
        public RestaurantListView()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            RestaurantViewModel = new RestaurantListViewModel(Navigation, this);
            BindingContext = RestaurantViewModel;
            InitializeComponent();
            var IsAdmin = DependencyService.Get<DependencyServices.ISettingService>().GetValueOrDefault<string>(Configuration.IS_ADMIN);
            if (IsAdmin.ToLower().Equals("true"))
            {
                addButton.IsVisible = true;
            }
            else
            {
                addButton.IsVisible = false;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            FetchDairyFarms();
            searchView.Text = "";
        }
        private async Task FetchDairyFarms()
        {
            await RestaurantViewModel.GetRestaurants();
        }
        public void ShowFarms(bool _isVisisble)
        {
            NoList.IsVisible = !_isVisisble;
            listLayout.IsVisible = true;
        }
        //Edit
        async void TapGestureRecognizer_Tapped(System.Object sender, System.EventArgs e)
        {
            if (Configuration.IsNetworkAvailable())
            {
                var obj = (int)((TappedEventArgs)e).Parameter;
                await Navigation.PushAsync(new AddNewRestaurant(RestaurantViewModel.RestaurantsList.ToList().Find(x => x.Id == obj)));
            }
            else
            {
                await DisplayAlert("Alert", Configuration.NetworkAlert, "OK");
            }
        }
        //Delete
        async void TapGestureRecognizer_Tapped_1(System.Object sender, System.EventArgs e)
        {
            if (Configuration.IsNetworkAvailable())
            {
                var response = await DisplayAlert("Delete", "Do you want to delete this restaurant?", "Yes", "No");
                if (response == true)
                {
                    var obj = (int)((TappedEventArgs)e).Parameter;
                    await RestaurantViewModel.DeleteRestaurant(obj);
                }
            }
            else
            {
                await DisplayAlert("OK", Configuration.NetworkAlert, "OK");
            }
        }

        void searchView_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.NewTextValue))
            {
                var result = new ObservableCollection<Models.Restaurant>();
                if (RestaurantViewModel.RestaurantsListFromServer != null)
                {
                    foreach (var item in RestaurantViewModel.RestaurantsListFromServer)
                    {
                        if (!string.IsNullOrEmpty(item.Address))
                        {
                            if (item.DisplayName.ToLower().Contains(e.NewTextValue.ToLower()) || item.Address.ToLower().Contains(e.NewTextValue.ToLower()))
                            {
                                result.Add(item);
                            }
                        }
                    }
                }

                RestaurantViewModel.RestaurantsList = result;
                ShowFarms(result.Count > 0);
            }
            else
            {
                if (RestaurantViewModel.RestaurantsListFromServer == null)
                {
                    RestaurantViewModel.RestaurantsListFromServer = new ObservableCollection<Models.Restaurant>();

                }
                RestaurantViewModel.RestaurantsList = RestaurantViewModel.RestaurantsListFromServer;
                ShowFarms(RestaurantViewModel.RestaurantsList.Count > 0);
            }
        }

        async void dataListview_ItemTapped(System.Object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            var selectedItem = (Models.Restaurant)e.Item;
            await Navigation.PushAsync(new RestaurantDetails(selectedItem.DisplayName));

        }

        async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}

