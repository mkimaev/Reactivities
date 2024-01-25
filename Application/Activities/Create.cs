using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Persistence;

namespace Application.Activities
{
    public class Create
    {
        public class Command : IRequest
        {
            public Activity Activity { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext dataContext;

            public Handler(DataContext dataContext)
            {
                this.dataContext = dataContext;
            }

			async Task IRequestHandler<Command>.Handle(Command request, CancellationToken cancellationToken)
			{
				dataContext.Activities.Add(request.Activity);
				await dataContext.SaveChangesAsync();
			}
		}
    }
}