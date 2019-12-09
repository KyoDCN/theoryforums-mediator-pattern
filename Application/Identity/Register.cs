using Application.Contract;
using Application.Error;
using Domain.Identity;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Identity
{
    public class Register
    {
        public class Command : IRequest<RegisterDTO>
        {
            public string UserName { get; set; }
            public string DisplayName { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class ValidateCommand : AbstractValidator<Command>
        {
            public ValidateCommand()
            {
                RuleFor(x => x.UserName)
                    .NotEmpty().WithMessage("Username is Required.")
                    .Matches(@"^[\w]*[^_\W]").WithMessage("Can only contain letters and numbers.");

                RuleFor(x => x.DisplayName)
                    .NotEmpty().WithMessage("Display Name is required.")
                    .MinimumLength(3).WithMessage("Display Name must be at least 3 characters long.")
                    .MaximumLength(26).WithMessage("Display Name cannot exceed 26 characters.")
                    .Matches(@"^[\w ]*[^_\W]").WithMessage("Can only contain letters, numbers, and spaces.")
                    .NotEqual(x => x.UserName).WithMessage("Display Name must not match Username");

                RuleFor(x => x.Email)
                    .NotEmpty().WithMessage("Email is required.")
                    .EmailAddress().WithMessage("Invalid Email input.");

                RuleFor(x => x.Password)
                    .NotEmpty().WithMessage("Password is required.")
                    .MinimumLength(8).WithMessage("Password must be at least 8 characters long.");
            }
        }

        public class Handler : IRequestHandler<Command, RegisterDTO>
        {
            private readonly UserManager<User> _userManager;
            private readonly ISecurity _security;

            public Handler(UserManager<User> userManager, ISecurity security)
            {
                _userManager = userManager;
                _security = security;
            }

            public async Task<RegisterDTO> Handle(Command request, CancellationToken cancellationToken)
            {
                User newUser = new User()
                {
                    UserName = request.UserName,
                    DisplayName = request.DisplayName,
                    Email = request.Email,
                    JoinDate = DateTime.Now
                };

                IdentityResult result = await _userManager.CreateAsync(newUser, request.Password);

                if (!result.Succeeded)
                    throw new RestException(HttpStatusCode.BadRequest, result.Errors);

                result = await _userManager.AddToRoleAsync(newUser, "Member");

                if (!result.Succeeded)
                    throw new RestException(HttpStatusCode.BadRequest, result.Errors);

                return new RegisterDTO { Token = await _security.GenerateLoginTokenAsync(newUser), AvatarUrl = newUser.AvatarUrl };
            }
        }
    }
}
