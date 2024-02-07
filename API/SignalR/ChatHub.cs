﻿
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Comments = Application.Comments;

namespace API.SignalR
{
	public class ChatHub: Hub
	{
		private readonly IMediator mediator;

		public ChatHub(IMediator mediator)
        {
			this.mediator = mediator;
		}

		public async Task SendComment(Comments.Create.Command command)
		{
			var comment = await mediator.Send(command);

			await Clients.Group(command.ActivityId.ToString())
				.SendAsync("ReceiveComment", comment.Value);
		}

		public override async Task OnConnectedAsync()
		{
			var httpContext = Context.GetHttpContext();
			var activityId = httpContext.Request.Query["activityId"];
			await Groups.AddToGroupAsync(Context.ConnectionId, activityId);
			var result = await mediator.Send(new Application.Comments.List.Query { ActivityId = Guid.Parse(activityId) });
			await Clients.Caller.SendAsync("LoadComments", result.Value);
		}
    }
}