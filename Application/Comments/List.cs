
using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;


namespace Application.Comments
{
	public class List
	{
		public class Query : IRequest<Result<List<CommentDto>>> 
		{
            public Guid ActivityId { get; set; }
        }

		public class Handler : IRequestHandler<Query, Result<List<CommentDto>>>
		{
			private readonly DataContext dataContext;
			private readonly IMapper mapper;

			public Handler(DataContext dataContext, IMapper mapper)
			{
				this.dataContext = dataContext;
				this.mapper = mapper;
			}
			public async Task<Result<List<CommentDto>>> Handle(Query request, CancellationToken cancellationToken)
			{
				var comments = await dataContext.Comments
					.Where(x => x.Activity.Id == request.ActivityId)
					.OrderByDescending(x => x.CreatedAt)
					.ProjectTo<CommentDto>(mapper.ConfigurationProvider)
					.ToListAsync(cancellationToken);

				return Result<List<CommentDto>>.Success(comments);
			}
		}
	}
}
