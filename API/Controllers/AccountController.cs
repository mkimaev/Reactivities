using API.DTOs;
using API.Services;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
	[AllowAnonymous]
	[ApiController]
	[Route("api/[controller]")]
	public class AccountController : ControllerBase
	{
		private readonly UserManager<AppUser> userManager;
		private readonly TokenService tokenService;

		public AccountController(UserManager<AppUser> userManager, TokenService tokenService)
        {
			this.userManager = userManager;
			this.tokenService = tokenService;
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
					Token = tokenService.CreateToken(user),
					Username = user.UserName
				};
			}

			return Unauthorized();
		}
    }
}
