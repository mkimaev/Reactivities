using API.DTOs;
using API.Services;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace API.Controllers
{
	[AllowAnonymous]
	public class FallbackController : Controller
	{
		public IActionResult Index()
		{
			var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "index.html");
			return PhysicalFile(filePath, "text/html");
		}
	}
}
