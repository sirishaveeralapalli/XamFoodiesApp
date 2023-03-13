using System;
using XamFoodiesApp.Droid;
using Android.Net;
using Android.App;
using Android.Content;
using Android.Util;
using XamFoodiesApp.DependencyServices;

[assembly: Xamarin.Forms.Dependency (typeof(NetworkService_Android))]
namespace XamFoodiesApp.Droid
{
	public class NetworkService_Android : INetworkService
	{
		const string TAG = "NetworkService_Android";
		ConnectivityReceiver connectivityReceiver;

		public NetworkService_Android ()
		{
			connectivityReceiver = new ConnectivityReceiver(this);
			Android.App.Application.Context.RegisterReceiver(connectivityReceiver, new IntentFilter(ConnectivityManager.ConnectivityAction));
		}

		public event EventHandler OnNetworkChange;


		public bool CheckInternetConnection ()
		{
			try {
				var connectivityManager = (ConnectivityManager)Application.Context.GetSystemService (Context.ConnectivityService);
				var activeConnection = connectivityManager.ActiveNetworkInfo;
				if ((activeConnection != null) && activeConnection.IsConnected) {
					return true;
				} else {
					return false;
				}
			} catch (Java.Lang.Exception ex) {
				Log.Debug (TAG, "CheckInternetConnection Exception:: " + ex.Message);
				return false;
			}
		}


		public void NotifyNetworkChange(Intent it)
		{ 
			ConnectivityManager connectivityManager = (ConnectivityManager)Application.Context.GetSystemService(Context.ConnectivityService);
			var connection = connectivityManager.ActiveNetworkInfo;
			if (connection != null && (connection.IsConnected || connection.IsConnectedOrConnecting))
			{
				if (OnNetworkChange != null)
				{
					OnNetworkChange(this, null);
				}
			}
			else {
				if (OnNetworkChange != null)
				{
					OnNetworkChange(this, null);
				}
			}

		}
	}

	class ConnectivityReceiver : BroadcastReceiver
	{
		NetworkService_Android mRef;
		public ConnectivityReceiver(NetworkService_Android refe)
		{
			mRef = refe;
		}

		public override void OnReceive(Context context, Intent intent)
		{
			mRef.NotifyNetworkChange(intent);
		}
	}
}

