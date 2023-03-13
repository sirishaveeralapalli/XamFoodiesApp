using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace XamFoodiesApp.ViewModels
{
	public class RestaurantDetailsViewModel :BaseViewModel
	{
        private Views.RestaurantDetails RestDetailsViewRef;
        public RestaurantDetailsViewModel(INavigation navigation, Views.RestaurantDetails RestaurantView)
		{
            pageRef = RestDetailsViewRef = RestaurantView;
            navigationRef = navigation;
            Configuration.ScreenName = "RestaurantDetailsView";

        }
        public const string ItemsListPropertyName = "ItemsList";
        private ObservableCollection<Models.Item> itemsList = new ObservableCollection<Models.Item>();
        public ObservableCollection<Models.Item> ItemsList
        {
            get { return itemsList; }
            set { SetProperty(ref itemsList, value, ItemsListPropertyName); }
        }

        public const string OrderedItemsPropertyName = "OrderedItems";
        private ObservableCollection<Models.Item> orderedItems = new ObservableCollection<Models.Item>();
        public ObservableCollection<Models.Item> OrderedItems
        {
            get { return orderedItems; }
            set { SetProperty(ref orderedItems, value, ItemsListPropertyName); }
        }

        /// <summary>
        /// The back action.
        /// </summary>
        private Command backAction;
        private const string BackCommandPropertyName = "BackAction";

        public Command BackAction
        {
            get
            {
                return backAction ?? (backAction = new Command(ExecuteBackCommand));
            }
        }
        // <summary>
        /// This method is executed when back button is clicked 
        /// </summary>
        public async void ExecuteBackCommand()
        {
            var IsAdmin = DependencyService.Get<DependencyServices.ISettingService>().GetValueOrDefault<string>(Configuration.IS_ADMIN);
            if (IsAdmin.ToLower().Equals("false"))
            {
                var exitConfirmed = await RestDetailsViewRef.DisplayAlert("Your changes will be lost", "Are you sure you want to discard your changes?", "OK", "Cancel");
                if (exitConfirmed)
                {
                    await navigationRef.PopAsync();
                }
            }
            else
            {
                await navigationRef.PopAsync();
            }
        }

    }
}

