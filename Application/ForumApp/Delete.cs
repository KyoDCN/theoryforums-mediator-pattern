using Application.Error;
using Domain.Forum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.ForumApp
{
    public class Delete
    {
        public class Command : IRequest
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly AppDbContext _context;

            public Handler(AppDbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                Forum forumToDelete = await _context.Forums.SingleOrDefaultAsync(x => x.Id == request.Id);

                if (forumToDelete == null)
                    throw new RestException(HttpStatusCode.BadRequest, "Invalid forum");

                _context.Remove(forumToDelete);

                if (await _context.SaveChangesAsync() > 0)
                    return Unit.Value;
                else
                    throw new Exception("Problem saving changes");
            }
        }
    }
}
