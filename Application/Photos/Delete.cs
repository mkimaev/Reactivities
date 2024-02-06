using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Photos
{
	public class Delete
	{
		public class Command : IRequest<Result<Unit>>
		{
			public string Id { get; set; }
		}


		public class Handler : IRequestHandler<Command, Result<Unit>>
		{
			private readonly DataContext dataContex;
			private readonly IPhotoAccessor photoAccessor;
			private readonly IUserAccessor userAccessor;

			public Handler(DataContext dataContex, IPhotoAccessor photoAccessor, IUserAccessor userAccessor)
			{
				this.dataContex = dataContex;
				this.photoAccessor = photoAccessor;
				this.userAccessor = userAccessor;
			}
			public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
			{
				var user = await dataContex.Users.Include(p => p.Photos)
					.FirstOrDefaultAsync(x => x.UserName == userAccessor.GetUsername());

				if (user == null) { return null; }

				var photo = user.Photos.FirstOrDefault(x => x.Id == request.Id);

				if (photo == null) { return null; };

				if (photo.IsMain) 
				{ 
					return Result<Unit>.Failure("You can not delete your main photo"); 
				}

				var result = await photoAccessor.DeletePhoto(request.Id);

				if (result == null) 
				{ 
					return Result<Unit>.Failure("Problem deleting photo from Cloudary"); 
				}

				user.Photos.Remove(photo);

				var success = await dataContex.SaveChangesAsync() > 0;

				if (success)
				{
					return Result<Unit>.Success(Unit.Value);
				}

				return Result<Unit>.Failure("Problem deleting photo from API");
			}
		}
	}
}
