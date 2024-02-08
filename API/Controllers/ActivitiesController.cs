using Application.Activities;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
	public class ActivitiesController : BaseApiController
	{

		public ActivitiesController()
		{
		}

		[HttpGet] //api/activities
		public async Task<ActionResult<List<Activity>>> GetAcitivties([FromQuery] ActivityParams param)
		{
			return HandlePagedResult(await Mediator.Send(new List.Query { Params = param }));
		}

		[HttpGet("{id}")] //api/acitivities/id
		public async Task<IActionResult> GetActivity(Guid id)
		{
			var result = await Mediator.Send(new Details.Query { Id = id });
			return HandleResult(result);
		}

		[HttpPost] //api/acitivities/
		public async Task<IActionResult> CreateActivity(Activity activity)
		{
			var result = await Mediator.Send(new Create.Command { Activity = activity });
			return HandleResult(result);
		}

		[Authorize(Policy = "IsActivityHost")]
		[HttpPut("{id}")]
		public async Task<IActionResult> EditActivity(Guid id, Activity activity)
		{
			activity.Id = id;
			var result = await Mediator.Send(new Edit.Command { Activity = activity });
			return HandleResult(result);
		}

		[Authorize(Policy = "IsActivityHost")]
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteActivity(Guid id)
		{
			var result = await Mediator.Send(new Delete.CommandDelete { Id = id });
			return HandleResult(result);
		}

		[HttpPost("{id}/attend")] //api/acitivities/
		public async Task<IActionResult> Attend(Guid id)
		{
			var result = await Mediator.Send(new UpdateAttendance.Command { Id = id });
			return HandleResult(result);
		}
	}
}