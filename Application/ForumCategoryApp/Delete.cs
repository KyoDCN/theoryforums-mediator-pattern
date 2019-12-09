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

namespace Application.ForumCategoryApp
{
    public class Delete
    {
        public class Command : IRequest
        {
            public int Id { get; set; }
        }

        public class ValidateCommand : AbstractValidator<Command>
        {
            public ValidateCommand()
            {
                RuleFor(x => x.Id).Must(x => x >= 0).WithMessage("Invalid forum category id.");
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
                ForumCategory forumToDelete = await _context.ForumCategories.SingleOrDefaultAsync(x => x.Id == request.Id);

                if (forumToDelete == null)
                    throw new RestException(HttpStatusCode.BadRequest, "Forum category not found.");

                _context.Remove(forumToDelete);

                if (await _context.SaveChangesAsync() > 0)
                    return Unit.Value;
                else
                    throw new Exception("Problem saving changes");
            }
        }
    }
}
