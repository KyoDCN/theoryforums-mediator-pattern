using Application.Error;
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
    public class Update
    {
        public class Command : IRequest
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
        }

        public class ValidateCommand : AbstractValidator<Command>
        {
            public ValidateCommand()
            {
                RuleFor(x => x.Id).Must(x => x >= 0).WithMessage("Invalid forum category id.");
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
                var categoryToUpdate = await _context.ForumCategories.SingleOrDefaultAsync(x => x.Id == request.Id);

                if (categoryToUpdate == null)
                    throw new RestException(HttpStatusCode.BadRequest, "Forum category not found.");

                categoryToUpdate.Title = request.Title ?? categoryToUpdate.Title;
                categoryToUpdate.Description = request.Description ?? categoryToUpdate.Description;

                if (await _context.SaveChangesAsync() > 0)
                    return Unit.Value;
                else
                    throw new Exception("Problem saving changes");
            }
        }

    }
}
