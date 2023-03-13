using System;
using Xamarin.Forms;

namespace XamFoodiesApp.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public string VegOrNonVeg { get; set; }
        public string VegOrNonVegColor {
            get
            {
                if (VegOrNonVeg.Equals("Veg"))
                    return "#1D8348";
                else
                    return "#C10A0A ";
            }
        }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public bool IsAddingAllowed
        {
            get
            {
                var IsAdmin = DependencyService.Get<DependencyServices.ISettingService>().GetValueOrDefault<string>(Configuration.IS_ADMIN);
                if (IsAdmin.ToLower().Equals("true"))
                    return false;
                else
                    return true;
            }
        }
    }
}

