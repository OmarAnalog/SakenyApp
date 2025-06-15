using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Sakenny.Core.Models;
using Sakenny.Repository.Data;
using System.Text;

namespace SakennyProject.Helper
{
    public static class IdentityServices
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
        {
            // add usermanager to DI Container
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<SakennyDbContext>()
                .AddDefaultTokenProviders();
            // add authentication to DI Container
            services.AddAuthentication(options =>
            {
                // validation Authentication Scheme
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                // when Validation fails
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuers = [configuration["Jwt:issuer"], configuration["Jwt:issuer2"]],
                    ValidAudiences = [configuration["Jwt:Audiance"], configuration["Jwt:Audiance2"]],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                };
            }).AddGoogle(options =>
            {
                options.ClientId = configuration["Authentication:ClientId"];
                options.ClientSecret = configuration["Authentication:ClientSecret"];
                options.CallbackPath = "/signin-google";
            });
            services.AddAuthorization();
            return services;
        }
    }
}
