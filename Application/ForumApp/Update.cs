using Application.Error;
using Domain.Forum;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.ForumApp
{
    public class Update
    {
        public class Command : IRequest
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string ImageUrl { get; set; }
        }

        public class ValidateCommand : AbstractValidator<Command>
        {
            public ValidateCommand()
            {
                RuleFor(x => x.Title).NotEmpty();
            }
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
                Forum forumToUpdate = await _context.Forums.SingleOrDefaultAsync(x => x.Id == request.Id);

                if (forumToUpdate == null)
                    throw new RestException(HttpStatusCode.BadRequest, "Invalid forum.");

                forumToUpdate.Title = request.Title ?? forumToUpdate.Title;
                forumToUpdate.Description = request.Description ?? forumToUpdate.Description;
                forumToUpdate.Icon = request.ImageUrl ?? forumToUpdate.Icon;

                if (await _context.SaveChangesAsync() > 0)
                    return Unit.Value;
                else
                    throw new Exception("Problem saving changes");
            }
        }

    }
}
