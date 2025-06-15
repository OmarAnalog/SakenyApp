using Microsoft.AspNetCore.Identity;
using Sakenny.Core.Models;
using System.Security.Claims;
namespace Sakenny.Core.Services
{
    public interface ITokenService
    {
        Task<string> GetTokenAsync(UserManager<User> userManager, User user);
        string GenerateRefreshToken();
        ClaimsPrincipal? GetClaimsPrincipalFromExpiredToken(string token);
    }
}
