using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Profiles
{
	public class ListActivities
	{
		public class Query : IRequest<Result<List<UserActivityDto>>>
		{
			public string Predicate { get; set; }
			public string Username { get; set; }
		}

		public class Handler : IRequestHandler<Query, Result<List<UserActivityDto>>>
		{
			private readonly DataContext dataContex;
			private readonly IMapper mapper;
			private readonly IUserAccessor userAccessor;

			public Handler(DataContext dataContex, IMapper mapper, IUserAccessor userAccessor)
			{
				this.dataContex = dataContex;
				this.mapper = mapper;
				this.userAccessor = userAccessor;
			}
			public async Task<Result<List<UserActivityDto>>> Handle(Query request, CancellationToken cancellationToken)
			{

				var _params = request.Predicate;

				var query = dataContex.ActivityAttendees
					.Where(actAttendee => actAttendee.AppUser.UserName == request.Username)
					.OrderBy(actAttendee => actAttendee.Activity.Date)
					.ProjectTo<UserActivityDto>(mapper.ConfigurationProvider)
					.AsQueryable();

				query = request.Predicate switch
				{
					"past" => query.Where(a => a.Date <= DateTime.UtcNow),
					"hosting" => query.Where(a => a.HostUsername == request.Username),
					_ => query.Where(a => a.Date >= DateTime.UtcNow)
				};

				var activities = await query.ToListAsync();

				return Result<List<UserActivityDto>>.Success(activities);
			}
		}
	}
}
