
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using XamFoodiesApp.DependencyServices;

namespace XamFoodiesApp.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        protected INavigation navigationRef;
        protected Page pageRef;
        public BaseViewModel()
        {
        }

        /**
         * Initialization can go here for each page
         **/
        protected void InitPage()
        {

        }
        /**
         * Cleanup can go here for each page if any
         **/
        protected void CleanPage()
        {

        }

        private string title = string.Empty;
        public const string TitlePropertyName = "Title";

        /// <summary>
        /// Gets or sets the "Title" property
        /// </summary>
        /// <value>The title.</value>
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value, TitlePropertyName); }
        }

        private string subTitle = string.Empty;
        /// <summary>
        /// Gets or sets the "Subtitle" property
        /// </summary>
        public const string SubtitlePropertyName = "Subtitle";
        public string Subtitle
        {
            get { return subTitle; }
            set { SetProperty(ref subTitle, value, SubtitlePropertyName); }
        }

        private string icon = null;
        /// <summary>
        /// Gets or sets the "Icon" of the viewmodel
        /// </summary>
        public const string IconPropertyName = "Icon";
        public string Icon
        {
            get { return icon; }
            set { SetProperty(ref icon, value, IconPropertyName); }
        }

        private bool isBusy;
        /// <summary>
        /// Gets or sets if the view is busy.
        /// </summary>
        public const string IsBusyPropertyName = "IsBusy";
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value, IsBusyPropertyName); }
        }

        public const string RefreshPropertyName = "Refresh";
        private bool isRefresh;
        public bool Refresh
        {
            get { return isRefresh; }
            set { SetProperty(ref isRefresh, !isRefresh, RefreshPropertyName); }
        }

        protected void SetProperty<T>(
            ref T backingStore, T value,
            string propertyName,
            Action onChanged = null,
            Action<T> onChanging = null)
        {
            try
            {
                if (EqualityComparer<T>.Default.Equals(backingStore, value))
                    return;

                if (onChanging != null)
                    onChanging(value);

                OnPropertyChanging(propertyName);

                backingStore = value;

                if (onChanged != null)
                    onChanged();

                OnPropertyChanged(propertyName);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("SetProperty Exception message = " + ex.Message);
            }
        }

        #region INotifyPropertyChanging implementation
        public event Xamarin.Forms.PropertyChangingEventHandler PropertyChanging;
        #endregion
        // <summary>
        /// This method is executed when a property is changed
        /// </summary>
        ///<param name="propertyName"> This parameter holds the name of the property </param>
        public void OnPropertyChanging(string propertyName)
        {
            try
            {
                if (PropertyChanging == null)
                    return;

                PropertyChanging(this, new Xamarin.Forms.PropertyChangingEventArgs(propertyName));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("OnPropertyChanging Exception message = " + ex.Message);
            }

        }


        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
        // <summary>
        /// This method is executed when a property is changed
        /// </summary>
        ///<param name="propertyName"> This parameter holds the name of the property </param>
        public void OnPropertyChanged(string propertyName)
        {
            try
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("OnPropertyChanged Exception message = " + ex.Message);
            }
        }

        #region Dependency services

        protected IProgressService IProgressService
        {
            get { return DependencyService.Get<IProgressService>(); }
            set { }
        }

        protected ISettingService ISettingService
        {
            get { return DependencyService.Get<ISettingService>(); }
            set { }
        }

        #endregion

    }
}
