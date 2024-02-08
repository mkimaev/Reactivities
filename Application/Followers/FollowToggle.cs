
using Application.Core;
using Application.Interfaces;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Followers
{
	public class FollowToggle
	{
		public class Command : IRequest<Result<Unit>>
		{
			public string TargetUsername { get; set; }
		}

		public class CommandValidator : AbstractValidator<Command>
		{
			public CommandValidator()
			{
				RuleFor(item => item.TargetUsername).NotEmpty();
			}
		}

		public class Handler : IRequestHandler<Command, Result<Unit>>
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

			public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
			{
				var observer = await dataContext.Users.FirstOrDefaultAsync(x => x.UserName == userAccessor.GetUsername());

				var target = await dataContext.Users.FirstOrDefaultAsync(x => x.UserName == request.TargetUsername);

				if (target == null) { return null; }

				var following = await dataContext.UserFollowings.FindAsync(observer.Id, target.Id);

				if (following == null) 
				{
					following = new UserFollowing
					{
						Observer = observer,
						Target = target
					};

					dataContext.UserFollowings.Add(following);
				}
				else
				{
					dataContext.UserFollowings.Remove(following);
				}

				var success = await dataContext.SaveChangesAsync() > 0;

				if (success)
				{
					return Result<Unit>.Success(Unit.Value);
				}

				return Result<Unit>.Failure("Failed to update following");
			}
		}
	}
}
