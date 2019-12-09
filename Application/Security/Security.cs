using Application.Contract;
using Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Security
{
    public class Security : ISecurity
    {
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _config;

        public Security(SignInManager<User> signInManager, IConfiguration config)
        {
            _signInManager = signInManager;
            _config = config;
        }

        public async Task<string> GenerateLoginTokenAsync(User user)
        {
            List<Claim> claims = (await _signInManager.CreateUserPrincipalAsync(user)).Claims.ToList();

            for (int i = 0; i < claims.Count; i++)
            {
                if (claims[i].Type == ClaimTypes.Name)
                {
                    claims[i] = new Claim(ClaimTypes.Name, user.DisplayName);
                    break;
                }
            }

            return new JwtSecurityTokenHandler().GenerateToken(claims, _config);
        }
    }

    internal static class SecurityExtensions
    {
        public static string GenerateToken(this JwtSecurityTokenHandler tokenHandler, IEnumerable<Claim> claims, IConfiguration config)
        {
            var key = Encoding.ASCII.GetBytes(config["AppSettings:TokenKey"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = config["AppSettings:Issuer"],
                Audience = config["AppSettings:Audience"],
                IssuedAt = DateTime.UtcNow,
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature),
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
