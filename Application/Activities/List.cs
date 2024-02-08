using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities
{
	public class List
	{
		public class Query : IRequest<Result<PagedList<ActivityDto>>> 
		{
			public ActivityParams Params { get; set; }
		}


		public class Handler : IRequestHandler<Query, Result<PagedList<ActivityDto>>>
		{
			private readonly DataContext dataContext;
			private readonly IMapper mapper;
			private readonly IUserAccessor userAccessor;

			public Handler(DataContext dataContext, IMapper mapper, IUserAccessor userAccessor)
			{
				this.dataContext = dataContext;
				this.mapper = mapper;
				this.userAccessor = userAccessor;
			}
			public async Task<Result<PagedList<ActivityDto>>> Handle(Query request, CancellationToken cancellationToken)
			{
				var _params = request.Params;
				var query = dataContext.Activities
					.Where(activity => activity.Date >= request.Params.StartDate)
					.OrderBy(activity => activity.Date)
					.ProjectTo<ActivityDto>(mapper.ConfigurationProvider,
						new { currentUsername = userAccessor.GetUsername() })
					.AsQueryable();

				if (request.Params.IsGoing && !request.Params.IsHost)
				{
					query= query.Where(activity => activity.Attendees.Any(a => a.Username == userAccessor.GetUsername()));
				}

				if (request.Params.IsHost && !request.Params.IsGoing)
				{
					query = query.Where(activity => activity.HostUsername == userAccessor.GetUsername());
				}

				return Result<PagedList<ActivityDto>>.Success(await PagedList<ActivityDto>.CreateAsync(query, _params.PageNumber, _params.PageSize));
			}
		}
	}
}