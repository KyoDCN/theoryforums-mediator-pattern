using Application.Contract;
using Application.Identity;
using Application.Security;
using AspServer.Authorization;
using AspServer.Middleware;
using Domain.Identity;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Persistence;
using System.Text;

namespace AspServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(x => {
                // Users by default must be authenticated to use controllers
                // Implement custom Authorization Requirements here
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .AddRequirements(new ValidateSecurityStampRequirement())
                    .Build();

                x.Filters.Add(new AuthorizeFilter(policy));
                
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
            .AddJsonOptions(x => x.JsonSerializerOptions.IgnoreNullValues = true)
            .AddFluentValidation(x =>
            {
                x.RegisterValidatorsFromAssemblyContaining(typeof(Register));
            });

            // IAuthorizationHandler Registrations
            services.AddScoped<IAuthorizationHandler, ValidateSecurityStampHandler>();

            services.AddCors(x =>
            {
                x.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200");
                });
            });

            services.AddDbContext<AppDbContext>(x => {
                x.UseSqlite(
                    Configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly("Persistence")
                );
            });

            services.AddIdentityCore<User>(opt =>
            {
                opt.Password.RequireDigit = false;
                opt.Password.RequiredLength = 8;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;
                opt.User.RequireUniqueEmail = true;
                opt.ClaimsIdentity.SecurityStampClaimType = "security_stamp";
            })
            .AddRoles<Role>()
            .AddSignInManager<SignInManager<User>>()
            .AddUserManager<UserManager<User>>()
            .AddRoleManager<RoleManager<Role>>()
            .AddUserValidator<UserValidator<User>>()
            .AddRoleValidator<RoleValidator<Role>>()
            .AddEntityFrameworkStores<AppDbContext>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(x =>
            {
                byte[] key = Encoding.ASCII.GetBytes(Configuration["AppSettings:TokenKey"]);

                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    RequireExpirationTime = true,
                    RequireSignedTokens = true,
                    ValidateAudience = true,
                    RequireAudience = true,
                    ValidIssuer = Configuration["AppSettings:Issuer"],
                    ValidAudience = Configuration["AppSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });

            services.AddAuthorization(options => {
                options.AddPolicy("AdminOnly", policy => policy.RequireRole("Administrator"));
                options.AddPolicy("StaffOnly", policy => policy.RequireRole("Admin", "Moderator"));
                options.AddPolicy("MemberOnly", policy => policy.RequireRole("Admin", "Moderator", "Member"));
            });

            services.AddMediatR(typeof(Register.Handler).Assembly);

            services.AddScoped<ISecurity, Security>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRestExceptionHandler();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors("CorsPolicy");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
