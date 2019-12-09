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
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Application.PostApp
{
    public class Create
    {
        public class Command : IRequest
        {
            public int ThreadId { get; set; }
            public string Content { get; set; }
        }

        public class ValidateCommand : AbstractValidator<Command>
        {
            public ValidateCommand()
            {
                RuleFor(x => x.ThreadId).GreaterThanOrEqualTo(0);
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

                var thread = await _context.Threads.SingleOrDefaultAsync(x => x.Id == request.ThreadId);

                if(thread == null)
                    throw new RestException(HttpStatusCode.BadRequest, "Invalid thread.");

                var newPost = new Post
                {
                    Content = JsonSerializer.Serialize(request.Content),
                    CreatedOn = DateTime.Now,
                    ThreadFK = thread.Id,
                    Thread = thread,
                    AuthorFK = user.Id,
                    Author = user
                };

                thread.LastUpdated = DateTime.Now;

                _context.Add(newPost);

                if (await _context.SaveChangesAsync() > 0)
                    return Unit.Value;
                else
                    throw new Exception("Problem saving changes");
            }
        }

    }
}
