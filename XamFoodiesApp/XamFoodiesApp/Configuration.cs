using System;
using Xamarin.Forms;
using XamFoodiesApp.DependencyServices;

namespace XamFoodiesApp
{
    public static class Configuration
    {
        public static string ScreenName = "screenname";
        public static string LOGGED_IN_USER_ID = "loggedinuserID";
        public static string IS_ADMIN = "false";

        public static string BaseUrl = "https://xamfoodiesapi.azurewebsites.net/";

        public static string NetworkAlert = "No Internet available";

        public static bool IsNetworkAvailable()
        {
            INetworkService netWorkService = DependencyService.Get<INetworkService>();
            bool isInternetAvailable = netWorkService.CheckInternetConnection();
            return isInternetAvailable;
        }
    }
}

