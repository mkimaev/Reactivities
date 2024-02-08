using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Followers
{
	public class List
	{
		public class Query : IRequest<Result<List<Profiles.Profile>>>
		{
			public string Predicate { get; set; }
			public string Username { get; set; }
		}

		public class CommandValidator : AbstractValidator<Query>
		{
			public CommandValidator()
			{
				RuleFor(item => item.Username).NotEmpty();
			}
		}

		public class Handler : IRequestHandler<Query, Result<List<Profiles.Profile>>>
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

			public async Task<Result<List<Profiles.Profile>>> Handle(Query request, CancellationToken cancellationToken)
			{
				var profiles = new List<Profiles.Profile>();

				switch (request.Predicate)
				{
					case "followers":
						profiles = await dataContext.UserFollowings.Where(x => x.Target.UserName == request.Username)
							.Select(u => u.Observer)
							.ProjectTo<Profiles.Profile>(mapper.ConfigurationProvider, 
								new {currentUsername = userAccessor.GetUsername()})
							.ToListAsync();
						break;

					case "following":
						profiles = await dataContext.UserFollowings.Where(x => x.Observer.UserName == request.Username)
							.Select(u => u.Target)
							.ProjectTo<Profiles.Profile>(mapper.ConfigurationProvider, 
								new { currentUsername = userAccessor.GetUsername() })
							.ToListAsync();
						break;
					default:
						break;
				}

				return Result<List<Profiles.Profile>>.Success(profiles);
			}
		}
	}
}
