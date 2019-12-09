using Application.Error;
using Domain.Forum;
using Domain.Identity;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Net;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Application.ThreadApp
{
    public class Create
    {
        public class Command : IRequest
        {
            public int ForumId { get; set; }
            public string Title { get; set; }
            public string Content { get; set; }
        }

        public class ValidateCommand : AbstractValidator<Command>
        {
            public ValidateCommand()
            {
                RuleFor(x => x.ForumId).GreaterThanOrEqualTo(0);
                RuleFor(x => x.Title).NotEmpty().MaximumLength(90);
                RuleFor(x => x.Content).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly AppDbContext _context;
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly SignInManager<User> _signInManager;

            public Handler(AppDbContext context, IHttpContextAccessor httpContextAccessor, SignInManager<User> signInManager)
            {
                _context = context;
                _httpContextAccessor = httpContextAccessor;
                _signInManager = signInManager;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                ClaimsPrincipal claimsPrincipal = _httpContextAccessor.HttpContext.User;

                User user = await _signInManager.ValidateSecurityStampAsync(claimsPrincipal);

                if (user == null)
                    throw new RestException(HttpStatusCode.Unauthorized);

                Forum forum = await _context.Forums.SingleOrDefaultAsync(x => x.Id == request.ForumId);

                if (forum == null)
                    throw new RestException(HttpStatusCode.BadRequest, "Invalid forum.");

                var newThread = new Domain.Forum.Thread
                {
                    Title = request.Title,
                    Content = request.Content,
                    CreatedOn = DateTime.Now,
                    LastUpdated = DateTime.Now,
                    ForumFK = forum.Id,
                    Forum = forum,
                    Edited = false,
                    AuthorFK = user.Id,
                    Author = user
                };

                _context.Add(newThread);

                if (await _context.SaveChangesAsync() > 0)
                    return Unit.Value;
                else
                    throw new Exception("Problem saving changes");
            }
        }

    }
}
