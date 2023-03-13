using System;
namespace XamFoodiesApp.Models
{
	public class User
	{
		public int Id { get; set; }
		public string DisplayName { get; set; }
		public string ModifiedAt { get; set; }
		public int ModifiedBy { get; set; }
		public string CreatedAt { get; set; }
		public int CreatedBy { get; set; }
        public bool IsDeleted { get; set; }
		public string Username { get; set; }
		public string Email { get; set; }
		public bool IsAdmin { get; set; }
    }

    public class UserCredential
    {
		public string username { get; set; }
		public string password { get; set; }
	}
}

