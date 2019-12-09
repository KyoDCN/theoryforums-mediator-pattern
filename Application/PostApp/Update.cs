using Application.Error;
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
    public class Update
    {
        public class Command : IRequest
        {
            public int PostId { get; set; }
            public string Content { get; set; }
        }

        public class ValidateCommand : AbstractValidator<Command>
        {
            public ValidateCommand()
            {
                RuleFor(x => x.PostId).GreaterThanOrEqualTo(0);
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

                // Does User exist?
                if (user == null)
                    throw new RestException(HttpStatusCode.Unauthorized);

                var postToUpdate = await _context.Posts.SingleOrDefaultAsync(x => x.Id == request.PostId);
                
                if (postToUpdate.Content == JsonSerializer.Serialize(request.Content))
                    return Unit.Value;

                // Does Post exist?
                if (postToUpdate == null)
                    throw new RestException(HttpStatusCode.BadRequest, "Post does not exist");

                bool isAdmin = claimsPrincipal.IsInRole("Administrator");

                // Is User an Admin or the Post's Owner?
                if (isAdmin == false || postToUpdate.AuthorFK != user.Id)
                    throw new RestException(HttpStatusCode.Unauthorized);

                postToUpdate.Content = JsonSerializer.Serialize(request.Content);
                postToUpdate.Edited = true;
                postToUpdate.LastEditDate = DateTime.Now;

                if (await _context.SaveChangesAsync() > 0)
                    return Unit.Value;
                else
                    throw new Exception("Problem saving changes");
            }
        }
    }
}
