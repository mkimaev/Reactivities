using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Persistence;

namespace Application.Activities
{
    public class Delete
    {
        public class CommandDelete : IRequest 
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<CommandDelete>
        {
            private readonly DataContext dataContext;

            public Handler(DataContext dataContext)
            {
                this.dataContext = dataContext;

            }

            public async Task Handle(CommandDelete request, CancellationToken cancellationToken)
            {
                var activity = await dataContext.Activities.FindAsync(request.Id);
                dataContext.Remove(activity);
                await dataContext.SaveChangesAsync();
            }
        }
    }
}