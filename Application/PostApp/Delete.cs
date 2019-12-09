using Application.Error;
using Domain.Forum;
using Domain.Identity;
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

namespace Application.PostApp
{
    public class Delete
    {
        public class Command : IRequest
        {
            public int PostId { get; set; }
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

                // Does User Exist?
                if (user == null)
                    throw new RestException(HttpStatusCode.Unauthorized);

                Post postToDelete = await _context.Posts.SingleOrDefaultAsync(x => x.Id == request.PostId);

                // Does Post Exist?
                if (postToDelete == null)
                    throw new RestException(HttpStatusCode.BadRequest, "Invalid post.");

                bool isAdmin = claimsPrincipal.IsInRole("Administrator");

                // Is User an Admin or the Post's Owner?
                if (isAdmin == false || postToDelete.AuthorFK != user.Id)
                    throw new RestException(HttpStatusCode.Unauthorized);

                _context.Remove(postToDelete);

                if (await _context.SaveChangesAsync() > 0)
                    return Unit.Value;
                else
                    throw new Exception("Problem saving changes");
            }
        }

    }
}
