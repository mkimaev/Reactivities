using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Domain;
using MediatR;
using Persistence;

namespace Application.Activities
{
    public class Delete
    {
        public class CommandDelete : IRequest<Result<Unit>>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<CommandDelete, Result<Unit>>
        {
            private readonly DataContext dataContext;

            public Handler(DataContext dataContext)
            {
                this.dataContext = dataContext;

            }

            public async Task<Result<Unit>> Handle(CommandDelete request, CancellationToken cancellationToken)
            {
                var activity = await dataContext.Activities.FindAsync(request.Id);

                if (activity is null)
                {
                    return null;
                }

                dataContext.Remove(activity);
                bool result = await dataContext.SaveChangesAsync() > 0;

                if (!result){
                    return Result<Unit>.Failure("Failed to delete the activity");
                }

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}