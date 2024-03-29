﻿using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Profiles
{
	public class Details
	{
		public class Query : IRequest<Result<Profile>>
		{
			public string Username { get; set; }
		}

		public class Handler : IRequestHandler<Query, Result<Profile>>
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
			public async Task<Result<Profile>> Handle(Query request, CancellationToken cancellationToken)
			{
				var user = await dataContex.Users
					.ProjectTo<Profile>(mapper.ConfigurationProvider,
						new { currentUsername = userAccessor.GetUsername() })
					.SingleOrDefaultAsync(x => x.Username == request.Username);

				return Result<Profile>.Success(user);
			}
		}
	}
}
