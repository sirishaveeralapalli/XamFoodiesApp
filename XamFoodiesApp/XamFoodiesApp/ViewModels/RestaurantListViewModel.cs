using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamFoodiesApp.DependencyServices;
using XamFoodiesApp.Models;
namespace XamFoodiesApp.ViewModels
{
	public class RestaurantListViewModel : BaseViewModel
    {
        private Views.RestaurantListView RestViewRef;
        private HttpService _httpService;
        private Service.UserService service;
        public RestaurantListViewModel(INavigation navigation, Views.RestaurantListView RestaurantView)
		{
            pageRef = RestViewRef = RestaurantView;
            navigationRef = navigation;
            Configuration.ScreenName = "RestaurantListView";
            _httpService = new HttpService();
        _httpService.DefaultHeader.Clear();

            service = new Service.UserService(_httpService);
    }
        public const string RestaurantsListPropertyName = "RestaurantsList";
        private ObservableCollection<Models.Restaurant> restaurantsList = new ObservableCollection<Models.Restaurant>();
        public ObservableCollection<Models.Restaurant> RestaurantsList
        {
            get { return restaurantsList; }
            set { SetProperty(ref restaurantsList, value, RestaurantsListPropertyName); }
        }

        public const string RestaurantsListFromServerPropertyName = "RestaurantsListFromServer";
        private ObservableCollection<Models.Restaurant> restaurantsListFromServer = new ObservableCollection<Models.Restaurant>();
        public ObservableCollection<Models.Restaurant> RestaurantsListFromServer
        {
            get { return restaurantsListFromServer; }
            set { SetProperty(ref restaurantsListFromServer, value, RestaurantsListFromServerPropertyName); }
        }
        /// <summary>
        /// The add dairy clicked.
        /// </summary>
        private Command addRestaurantClicked;
        public const string AddRestaurantClickedPropertyName = "AddRestaurantClicked";

        public Command AddRestaurantClicked
        {
            get
            {
                return addRestaurantClicked ?? (addRestaurantClicked = new Command(ExecuteAddRestaurantClicked));
            }
        }
        // <summary>
        /// This method adds a new dairy
        /// </summary>
        public async void ExecuteAddRestaurantClicked()
        {
            if (IsBusy)
                return;
            IsBusy = true;
            if (Configuration.IsNetworkAvailable())
            {
                try
                {
                   await navigationRef.PushAsync(new Views.AddNewRestaurant(null));
                }
                catch (Exception ex)
                {

                }
            }
            else
            {
                await RestViewRef.DisplayAlert("Alert", Configuration.NetworkAlert, "OK");
            }
            IsBusy = false;
        }

        // <summary>
        /// This method fetches farms using the user ID
        /// </summary>
        public async Task GetRestaurants()
        {
            try
            {
                    if (Configuration.IsNetworkAvailable())
                    {
                        DependencyService.Get<IProgressService>().ShowProgress("");
                        var result = new List<Restaurant>();
                            result = await service.GetRestaurants();
                        DependencyService.Get<IProgressService>().HideProgress();
                        if (result != null && result.Count > 0)
                        {
                            var records = new List<Restaurant>();
                            foreach (var item in result)
                            {
                                records.Add(item);
                            }
                            RestaurantsList = new ObservableCollection<Restaurant>(result);
                        RestaurantsListFromServer = restaurantsList;
                            RestViewRef.ShowFarms(true);
                        }
                        else
                        {
                            RestViewRef.ShowFarms(false);
                        }
                    }
                    else
                    {
                        await RestViewRef.DisplayAlert("Alert", "Unable to fetch restaurants. Please contact Admin.", "Ok");
                    }
            }
            catch (Exception ex)
            {
                await RestViewRef.DisplayAlert("Alert", "Unable to Add restaurant. Please contact Admin.", "Ok");
            }
        }

        internal async Task DeleteRestaurant(int obj)
        {
            try
            {
                DependencyService.Get<IProgressService>().ShowProgress("");
                var result = await service.DeleteRestaurant(obj);
                DependencyService.Get<IProgressService>().HideProgress();
                var item = RestaurantsList.ToList().Find(x => x.Id == obj);
                if (item != null)
                {
                    RestaurantsList.Remove(item); ;
                }
            }
            catch (Exception)
            {
                await RestViewRef.DisplayAlert("Alert", "Unable to delete the restaurant", "OK");
            }
        }
    }
}

