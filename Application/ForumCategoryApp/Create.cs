using Domain.Forum;
using FluentValidation;
using MediatR;
using Persistence;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.ForumCategoryApp
{
    public class Create
    {
        public class Command : IRequest
        {
            public string Title { get; set; }
            public string Description { get; set; }
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
                var newForumCategory = new ForumCategory
                {
                    Title = request.Title,
                    Description = request.Description ?? string.Empty
                };

                _context.Add(newForumCategory);

                if (await _context.SaveChangesAsync() > 0)
                    return Unit.Value;
                else
                    throw new Exception("Problem saving changes");
            }
        }
    }
}
