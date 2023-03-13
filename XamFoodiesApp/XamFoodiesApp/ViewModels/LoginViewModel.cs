using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamFoodiesApp.ViewModels
{
	public class LoginViewModel : BaseViewModel
    {
        private Views.LoginPage loginViewRef;
        private HttpService _httpService;
        private Service.UserService service;
        public LoginViewModel(INavigation navigation, Views.LoginPage loginView)
		{
            pageRef = loginViewRef = loginView;
            navigationRef = navigation;
            Configuration.ScreenName = "LoginView";
            _httpService = new HttpService();
            _httpService.DefaultHeader.Clear();

            service = new Service.UserService(_httpService);
        }

        /// <summary>
        /// The add dairy clicked.
        /// </summary>
        private Command loginClicked;
        public const string LoginClickedPropertyName = "LoginClicked";

        public Command LoginClicked
        {
            get
            {
                return loginClicked ?? (loginClicked = new Command(ExecuteLoginClicked));
            }
        }
        public const string UserNamePropertyName = "UserName";
        private string userName = string.Empty;
        public string UserName
        {
            get { return userName; }
            set { SetProperty(ref userName, value, UserNamePropertyName); }
        }

        public const string PasswordPropertyName = "Password";
        private string password = string.Empty;
        public string Password
        {
            get { return password; }
            set { SetProperty(ref password, value, PasswordPropertyName); }
        }
        // <summary>
        /// This method adds a new dairy
        /// </summary>
        public async void ExecuteLoginClicked()
        {
            if (string.IsNullOrEmpty(UserName))
            {
                await loginViewRef.DisplayAlert("Alert", "Please enter username", "OK");
                return;
            }
            if (string.IsNullOrEmpty(Password))
            {
                await loginViewRef.DisplayAlert("Alert", "Please enter password", "OK");
                return;
            }
            bool isValid = await Validate();
            if (isValid) { 
                await navigationRef.PushAsync(new Views.RestaurantListView());
            }
            else
            {
                await loginViewRef.DisplayAlert("Alert", "Please check your credentials", "OK");
                return;

            }
        }

        private async Task<bool> Validate()
        {
            bool isValid = false;
            if (Configuration.IsNetworkAvailable())
            {
                DependencyService.Get<DependencyServices.IProgressService>().ShowProgress("");
                var result = await service.UserValidate(UserName, Password);
                DependencyService.Get<DependencyServices.IProgressService>().HideProgress();
                if (result != null && result.Id > 0)
                {
                    ISettingService.AddOrUpdateValue(Configuration.LOGGED_IN_USER_ID, result.Id + "");
                    ISettingService.AddOrUpdateValue(Configuration.IS_ADMIN, result.IsAdmin+"");
                    ISettingService.Save();
                    isValid=true;
                }
                else
                {
                    isValid = false;
                }
            }
            else
            {
                isValid = false;
            }
            return isValid;
        }
                
    }
}

