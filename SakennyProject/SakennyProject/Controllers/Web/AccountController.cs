using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Sakenny.Core.Models;
using Sakenny.Core.Services;
using SakennyProject.DTO;
using SakennyProject.Erorrs;

namespace SakennyProject.Controllers.Web
{
    [Route("api/web/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly ITokenService tokenService;
        private readonly SignInManager<User> signInManager;
        private readonly IEmailService emailService;
        private readonly IConfiguration configuration;

        public AccountController(UserManager<User> userManager
            , ITokenService tokenService,
            SignInManager<User> signInManager
            , IEmailService emailService
            , IConfiguration configuration,
            RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.tokenService = tokenService;
            this.signInManager = signInManager;
            this.emailService = emailService;
            this.configuration = configuration;
        }

        [HttpPost("Login")]
        public async Task<ActionResult<ReturnedLogin>> login(LoginDto request)
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user is null) return Unauthorized("Invalid Email or Password");
            var IsAdmin = await userManager.IsInRoleAsync(user, configuration["Roles:Admin"]);
            var confirmSignIn = await userManager.CheckPasswordAsync(user, request.Password);
            if (!confirmSignIn)
                return Unauthorized(new ApiResponse(401,"Invalid Email or Password"));
            if (user.VerifiedAt is null)
                return Forbid("Email not verified. Please confirm your email.");
            if (!IsAdmin)
                return Forbid("Invalid  Account ");
            var confirmLogin = await signInManager.PasswordSignInAsync(user, request.Password, request.RememberMe, false);
            var refreshToken = tokenService.GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = request.RememberMe ? DateTime.Now.AddDays(30) : DateTime.Now.AddDays(1);
            await userManager.UpdateAsync(user);
            var returnedUser = new ReturnedLogin()
            {
                AccessToken = await tokenService.GetTokenAsync(userManager, user),
                RefreshToken = refreshToken,
                DisplayName = user.FirstName,
                Email = request.Email
            };
            return Ok(returnedUser);
        }

       
      
    }
}
