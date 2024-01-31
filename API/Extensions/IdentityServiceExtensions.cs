using API.Services;
using Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Routing.Tree;
using Microsoft.IdentityModel.Tokens;
using Persistence;
using System.Text;

namespace API.Extensions
{
	public static class IdentityServiceExtensions
	{
		public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
		{
			services.AddIdentityCore<AppUser>(opt =>
			{
				opt.Password.RequireNonAlphanumeric = false;
			})
			.AddEntityFrameworkStores<DataContext>();

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("BF8A*((,%c4Hubc5}9\"eNta#s]HY;rvQZ2Q+!ChD:RT[_h2Gs~h?z\\n-`u)X-Lje")); //default

			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(options =>
				{
					options.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuerSigningKey = true,
						IssuerSigningKey = key,
						ValidateIssuer = false,
						ValidateAudience = false
					};
				});
			services.AddScoped<TokenService>();

			return services;
		}

	}
}
