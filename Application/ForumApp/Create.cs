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
    public class Create
    {
        public class Command : IRequest
        {
            public int ForumCategoryId { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string Icon { get; set; }
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
                ForumCategory forumCategory = await _context.ForumCategories.SingleOrDefaultAsync(x => x.Id == request.ForumCategoryId);

                if (forumCategory == null)
                    throw new RestException(HttpStatusCode.BadRequest, "Invalid forum category.");

                var newForum = new Forum
                {
                    Title = request.Title,
                    Description = request.Description ?? string.Empty,
                    Icon = request.Icon ?? string.Empty,
                    ForumCategoryFK = forumCategory.Id,
                    ForumCategory = forumCategory
                };

                _context.Add(newForum);

                if (await _context.SaveChangesAsync() > 0)
                    return Unit.Value;
                else
                    throw new Exception("Problem saving changes");
            }
        }

    }
}
