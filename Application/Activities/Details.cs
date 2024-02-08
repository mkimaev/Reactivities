using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities
{
	public class Details
	{
		public class Query : IRequest<Result<ActivityDto>>
		{
			public Guid Id { get; set; }
		}

		public class Handler : IRequestHandler<Query, Result<ActivityDto>>
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

			public async Task<Result<ActivityDto>> Handle(Query request, CancellationToken cancellationToken)
			{
				var activity = await dataContext.Activities
					.ProjectTo<ActivityDto>(mapper.ConfigurationProvider, 
						new { currentUsername = userAccessor.GetUsername() })
					.FirstOrDefaultAsync(x => x.Id == request.Id);
				return Result<ActivityDto>.Success(activity);
			}
		}
	}
}