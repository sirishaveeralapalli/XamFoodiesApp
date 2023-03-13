using System;
using Xamarin.Forms;

namespace XamFoodiesApp.Models
{
    public class Restaurant
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public string Address { get; set; }
        public double PriceForTwo { get; set; }
        public int AdminId { get; set; }
        public bool IsEditable {
            get {
                var IsAdmin = DependencyService.Get<DependencyServices.ISettingService>().GetValueOrDefault<string>(Configuration.IS_ADMIN);
                if (IsAdmin.ToLower().Equals("true"))
                    return true;
                else
                    return false;
               }
        }
    }
}

