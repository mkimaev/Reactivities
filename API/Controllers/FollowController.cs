﻿using Followers = Application.Followers;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
	public class FollowController: BaseApiController
	{
		[HttpPost("{username}")]
		public async Task<IActionResult> Follow(string username)
		{
			return HandleResult(await Mediator.Send(new Followers.FollowToggle.Command { TargetUsername = username }));
		}

		[HttpGet("{username}")]
		public async Task<IActionResult> GetFollowings(string username, string predicate)
		{
			return HandleResult(await Mediator.Send(new Followers.List.Query 
			{ 
				Username = username, 
				Predicate = predicate 
			}));
		}
	}
}
