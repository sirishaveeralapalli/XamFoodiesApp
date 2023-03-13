using System;
using System.IO;
using System.Reflection.Emit;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamFoodiesApp.DependencyServices;
using XamFoodiesApp.Models;
using XamFoodiesApp.Service;
using static Xamarin.Essentials.Permissions;

namespace XamFoodiesApp.ViewModels
{
	public class AddNewRestViewModel :BaseViewModel
	{
        private Views.AddNewRestaurant addRestViewRef;
        private HttpService _httpService;
        private UserService service;
        public AddNewRestViewModel(INavigation navigation, Views.AddNewRestaurant addRestaurantView)
		{
            pageRef = addRestViewRef = addRestaurantView;
            navigationRef = navigation;
            Configuration.ScreenName = "AddRestaurantView";
            _httpService = new HttpService();
            _httpService.DefaultHeader.Clear();

            service = new UserService(_httpService);
        }

        public const string DisplayNamePropertyName = "DisplayName";
        private string displayName = string.Empty;
        public string DisplayName
        {
            get { return displayName; }
            set { SetProperty(ref displayName, value, DisplayNamePropertyName); }
        }

        public const string AddressLine1PropertyName = "AddressLine1";
        private string addressLine1 = string.Empty;
        public string AddressLine1
        {
            get { return addressLine1; }
            set { SetProperty(ref addressLine1, value, AddressLine1PropertyName); }
        }

        public const string AddressLine2PropertyName = "AddressLine2";
        private string addressLine2 = string.Empty;
        public string AddressLine2
        {
            get { return addressLine2; }
            set { SetProperty(ref addressLine2, value, AddressLine2PropertyName); }
        }

        public const string PriceForTwoPropertyName = "PriceForTwo";
        private double priceForTwo = 0;
        public double PriceForTwo
        {
            get { return priceForTwo; }
            set { SetProperty(ref priceForTwo, value, PriceForTwoPropertyName); }
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
          
                var exitConfirmed = await addRestViewRef.DisplayAlert("Your changes will be lost", "Are you sure you want to discard your changes?", "OK", "Cancel");
                if (exitConfirmed)
                {
                    await navigationRef.PopAsync();
                }
        }
        /// <summary>
        /// The save clicked.
        /// </summary>
        private Command saveClicked;
        public const string SaveClickedPropertyName = "SaveClicked";

        public Command SaveClicked
        {
            get
            {
                return saveClicked ?? (saveClicked = new Command(ExecuteSaveClicked));
            }
        }

        // <summary>
        /// This is the method that executes when 'Save' button is clicked
        /// </summary>
        public void ExecuteSaveClicked()
        {
            if (IsBusy)
                return;
            IsBusy = true;
            //navigationRef.PushAsync(new VerifyEmailView(false));
            if (Configuration.IsNetworkAvailable())
            {
                if (Validate())
                {
                    AddRestaurant();
                }
            }
            else
            {
                addRestViewRef.DisplayAlert("Alert", Configuration.NetworkAlert, "Ok");
            }
            IsBusy = false;
        }
        // <summary>
        /// This is the method that sends the data entered by the user to the service to be added as a new dairy
        /// </summary>
        /// <param name="IsAnimateused"> This is a boolean value that shows if user checked the 'do you use animate?' radio button </param>
        private async void AddRestaurant()
        {

            try
            {
                var userId = ISettingService.GetValueOrDefault<string>(Configuration.LOGGED_IN_USER_ID);

                DependencyService.Get<IProgressService>().ShowProgress("");

                Restaurant dto = new Restaurant();
                dto.Id = addRestViewRef.restaurantId;
                dto.DisplayName = displayName;
                dto.Address = addressLine1 + addressLine2;
                dto.PriceForTwo = priceForTwo;
                dto.AdminId = 2;
                var result = await service.AddRestaurant(dto);
                if (result != null && !result.IsError)
                {
                    await addRestViewRef.DisplayAlert("Alert", "The restaurant is saved successfully", "Ok");
                    await navigationRef.PopAsync();
                }
                else
                {
                    await addRestViewRef.DisplayAlert("Alert", result.ErrorMessage, "Ok");
                }
                DependencyService.Get<IProgressService>().HideProgress();
            }
            catch (Exception)
            {
                DependencyService.Get<IProgressService>().HideProgress();
                await addRestViewRef.DisplayAlert("Alert", "Unable to Add restaurant. Please contact Admin.", "Ok");
            }
        }
        private bool Validate()
        {
            if (string.IsNullOrEmpty(DisplayName))
            {
                addRestViewRef.DisplayAlert("Alert", "Please enter the name.", "Ok");
                return false;
            }
            if (string.IsNullOrEmpty(AddressLine1) && string.IsNullOrEmpty(AddressLine2))
            {
                addRestViewRef.DisplayAlert("Alert", "Please enter address.", "Ok");
                return false;
            }
            if (PriceForTwo==0 || priceForTwo==0.0)
            {
                addRestViewRef.DisplayAlert("Alert", "Please enter price for two.", "Ok");
                return false;
            }
            return true;
        }

        }
}

