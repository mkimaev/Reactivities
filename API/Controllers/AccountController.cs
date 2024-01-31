using API.DTOs;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class AccountController : ControllerBase
	{
		private readonly UserManager<AppUser> userManager;

		public AccountController(UserManager<AppUser> userManager)
        {
			this.userManager = userManager;
		}

		[HttpPost("login")]
		public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
		{
			var user = await this.userManager.FindByEmailAsync(loginDto.Email);
			
			if (user == null)
			{
				return Unauthorized();
			}

			var result = await userManager.CheckPasswordAsync(user, loginDto.Password);

			if (result)
			{
				return new UserDto
				{
					DisplayName = user.DisplayName,
					Image = null,
					Token = "this will be a token",
					Username = user.UserName
				};
			}

			return Unauthorized();
		}
    }
}
