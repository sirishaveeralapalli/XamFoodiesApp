using System;
using XamFoodiesApp.Droid;
using Android.App;
using AndroidHUD;
using Xamarin.Forms;
using XamFoodiesApp.DependencyServices;
using XamFoodiesApp.Droid;

[assembly: Xamarin.Forms.Dependency (typeof(ProgressService_Android))]
namespace XamFoodiesApp.Droid
{
	public class ProgressService_Android : IProgressService
	{
		public ProgressService_Android ()
		{
		}
		public void ShowProgress(string message){
			//AndHUD.Shared.Show(MainActivity.activity, "Status Message", MaskType.Clear);
			AndHUD.Shared.Show (MainActivity.context,message,-1,MaskType.Black,TimeSpan.FromSeconds (500),null, true, null);
		}

		public void HideProgress(){
			AndHUD.Shared.Dismiss (MainActivity.context);
		}

		public void ShowToast(string message){
			AndHUD.Shared.ShowToast (MainActivity.context,message,MaskType.Clear,TimeSpan.FromMilliseconds (2000),false,null,null);
		}

		public void ShowAlertWithTextField(string title, string message, Action<bool, string> Selection)
		{
			AlertDialog.Builder alert = new AlertDialog.Builder((Activity)Forms.Context);
			alert.SetTitle(title);
			alert.SetMessage(message);
            global::Android.Widget.EditText editText = new global::Android.Widget.EditText((Activity)Forms.Context);
            editText.Hint = "Email Address";
            //CustomEntry editText = new CustomEntry();
			alert.SetView(editText);
			alert.SetPositiveButton("OK", (object sender, Android.Content.DialogClickEventArgs e) =>
			{
				Console.WriteLine("OK Clicked");
				Selection(true, editText.Text);
			});
			alert.SetNegativeButton("Cancel", (object sender, Android.Content.DialogClickEventArgs e) =>
			{
				Console.WriteLine("Cancel Clicked");
				Selection(false, "Cancel");
			});
			alert.Show();
		}
	}
}

