using Application.Contract;
using Application.Error;
using Domain.Identity;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Identity
{
    public class Login
    {
        public class Query : IRequest<LoginDTO>
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        public class ValidateQuery : AbstractValidator<Query>
        {
            public ValidateQuery()
            {
                RuleFor(x => x.Username)
                    .NotEmpty().WithMessage("Username is Required.");

                RuleFor(x => x.Password)
                    .NotEmpty().WithMessage("Password is required.");
            }
        }

        public class Handler : IRequestHandler<Query, LoginDTO>
        {
            private readonly SignInManager<User> _signInManager;
            private readonly UserManager<User> _userManager;
            private readonly ISecurity _security;

            public Handler(SignInManager<User> signInManager, UserManager<User> userManager, ISecurity security)
            {
                _signInManager = signInManager;
                _userManager = userManager;
                _security = security;
            }

            public async Task<LoginDTO> Handle(Query request, CancellationToken cancellationToken)
            {
                User user = await _userManager.FindByNameAsync(request.Username);

                if (user == null)
                    throw new RestException(HttpStatusCode.BadRequest, "Incorrect Username or Password");

                SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

                if (!result.Succeeded)
                    throw new RestException(HttpStatusCode.BadRequest, "Incorrect Username or Password");

                // Log out all other sessions
                await _userManager.UpdateSecurityStampAsync(user);

                return new LoginDTO { Token = await _security.GenerateLoginTokenAsync(user), AvatarUrl = user.AvatarUrl };
            }
        }

    }
}
