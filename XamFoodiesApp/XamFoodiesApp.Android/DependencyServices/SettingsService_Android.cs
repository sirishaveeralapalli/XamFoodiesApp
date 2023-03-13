using System;
using Android.Content;
using Android.Preferences;
using Android.App;
using XamFoodiesApp.Droid;
using Android.Content.PM;
using Android.Views;
using XamFoodiesApp.DependencyServices;
using XamFoodiesApp.DependencyServices;

[assembly: Xamarin.Forms.Dependency(typeof(SettingsService_Android))]
namespace XamFoodiesApp.Droid
{
    public class SettingsService_Android : ISettingService
	{
		private readonly object _locker = new object();
		WindowManagerFlags _originalFlags;
		public SettingsService_Android()
		{
			SharedPreferences = PreferenceManager.GetDefaultSharedPreferences(Application.Context);
			SharedPreferencesEditor = SharedPreferences.Edit();
		}

		private static ISharedPreferences SharedPreferences { get; set; }
		private static ISharedPreferencesEditor SharedPreferencesEditor { get; set; }

		public T GetValueOrDefault<T>(string key, T defaultValue = default(T))
		{
			lock (_locker)
			{
				var typeOf = typeof (T);
				if (typeOf.IsGenericType && typeOf.GetGenericTypeDefinition() == typeof (Nullable<>))
				{
					typeOf = Nullable.GetUnderlyingType(typeOf);
				}
				object value = null;
				var typeCode = Type.GetTypeCode(typeOf);
				switch (typeCode)
				{
				case TypeCode.Boolean:
					value = SharedPreferences.GetBoolean(key, Convert.ToBoolean(defaultValue));
					break;
				case TypeCode.Int64:
					value = SharedPreferences.GetLong(key, Convert.ToInt64(defaultValue));
					break;
				case TypeCode.String:
					value = SharedPreferences.GetString(key, Convert.ToString(defaultValue));
					break;
				case TypeCode.Int32:
					value = SharedPreferences.GetInt(key, Convert.ToInt32(defaultValue));
					break;
				case TypeCode.Single:
					value = SharedPreferences.GetFloat(key, Convert.ToSingle(defaultValue));
					break;
				case TypeCode.DateTime:
					var ticks = SharedPreferences.GetLong(key, -1);
					if (ticks == -1)
						value = defaultValue;
					else
						value = new DateTime(ticks);
					break;
				}

				return null != value ? (T) value : defaultValue;
			}
		}

		public bool AddOrUpdateValue(string key, object value)
		{
			lock (_locker)
			{
                if(value == null){
                    return true;
                }
				var typeOf = value.GetType();
				if (typeOf.IsGenericType && typeOf.GetGenericTypeDefinition() == typeof (Nullable<>))
				{
					typeOf = Nullable.GetUnderlyingType(typeOf);
				}
				var typeCode = Type.GetTypeCode(typeOf);
				switch (typeCode)
				{
				case TypeCode.Boolean:
					SharedPreferencesEditor.PutBoolean(key, Convert.ToBoolean(value));
					break;
				case TypeCode.Int64:
					SharedPreferencesEditor.PutLong(key, Convert.ToInt64(value));
					break;
				case TypeCode.String:
					SharedPreferencesEditor.PutString(key, Convert.ToString(value));
					break;
				case TypeCode.Int32:
					SharedPreferencesEditor.PutInt(key, Convert.ToInt32(value));
					break;
				case TypeCode.Single:
					SharedPreferencesEditor.PutFloat(key, Convert.ToSingle(value));
					break;
				case TypeCode.DateTime:
					SharedPreferencesEditor.PutLong(key, ((DateTime) value).Ticks);
					break;
				}
			}

			return true;
		}

		public void Save()
		{
			lock (_locker)
			{
				SharedPreferencesEditor.Commit();
			}
		}

		public void CloseApp()
		{
			MainActivity.activity.Finish();
		}

        public void ChangeOrientation()
        {
            var activity = (Activity)Xamarin.Forms.Forms.Context;
            {
                activity.RequestedOrientation = ScreenOrientation.Portrait;
                var attrs = activity.Window.Attributes;
                attrs.Flags = _originalFlags;
                activity.Window.Attributes = attrs;
            }
        }

        public void ClearURLCache()
        {
        }

        public void ChangeLandscapeOrientation()
        {
            var activity = (Activity)Xamarin.Forms.Forms.Context;
            //if (orientationType.Equals("Landscape"))
            {
                activity.RequestedOrientation = ScreenOrientation.Landscape;
                var attrs = activity.Window.Attributes;
                _originalFlags = attrs.Flags;
                attrs.Flags |= Android.Views.WindowManagerFlags.Fullscreen;
                activity.Window.Attributes = attrs;
            }

        }

        public void UserOrientation ()
        {
            var activity = (Activity)Xamarin.Forms.Forms.Context;
            {
                activity.RequestedOrientation = ScreenOrientation.User;
                var attrs = activity.Window.Attributes;
                attrs.Flags = _originalFlags;
                activity.Window.Attributes = attrs;
            }
        }

        public string GetVersionNumber()
        {
            return MainActivity.context.PackageManager.GetPackageInfo(MainActivity.context.PackageName, 0).VersionName;
        }
	}
}

