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
using System.Threading;
using System.Threading.Tasks;

namespace Application.ThreadApp
{
    public class Update
    {
        public class Command : IRequest
        {
            public int Id { get; set; }
            public string Title { get; internal set; }
            public string Content { get; internal set; }
        }

        public class ValidateCommand : AbstractValidator<Command>
        {
            public ValidateCommand()
            {
                RuleFor(x => x.Title).NotEmpty();
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
                var threadToUpdate = await _context.Threads.SingleOrDefaultAsync(x => x.Id == request.Id);

                if (threadToUpdate == null)
                    throw new RestException(HttpStatusCode.BadRequest, "Invalid thread.");

                ClaimsPrincipal claimsPrincipal = _httpContextAccessor.HttpContext.User;

                User user = await _signInManager.ValidateSecurityStampAsync(claimsPrincipal);

                if (user == null)
                    throw new RestException(HttpStatusCode.Unauthorized);

                bool isAdmin = claimsPrincipal.IsInRole("Administrator");

                if (isAdmin == false || threadToUpdate.AuthorFK != user.Id)
                    throw new RestException(HttpStatusCode.Unauthorized);

                threadToUpdate.Title = request.Title ?? threadToUpdate.Title;

                if (threadToUpdate.Content != request.Content)
                {
                    threadToUpdate.Content = request.Content;
                    threadToUpdate.Edited = true;
                    threadToUpdate.LastEditDate = DateTime.Now;
                }

                if (await _context.SaveChangesAsync() > 0)
                    return Unit.Value;
                else
                    throw new Exception("Problem saving changes");
            }
        }

    }
}
