using Microsoft.AspNetCore.Identity;
using System;



namespace Domain
{
	public class AppUser: IdentityUser
	{
		public string DisplayName { get; set; }
        public string Bio { get; set; }
    }

}