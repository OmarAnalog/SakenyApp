using AutoMapper;
using Google.Apis.Auth;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Text;
using Newtonsoft.Json.Linq;
using Sakenny.Core.DTO;
using Sakenny.Core.Models;
using Sakenny.Core.Repositories;
using Sakenny.Core.Services;
using Sakenny.Services;
using Sakenny.Services.ImageService;
using SakennyProject.DTO;
using SakennyProject.Erorrs;
using System.Security.Claims;

namespace SakennyProject.Controllers.Mobile
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly ITokenService tokenService;
        private readonly SignInManager<User> signInManager;
        private readonly IEmailService emailService;
        private readonly IConfiguration configuration;
        private readonly ImageService imageService;
        private readonly IMapper mapper;
        private readonly IGenericRepository<FavouriteList> favRepository;

        public AccountController(UserManager<User> userManager
            , ITokenService tokenService,
            SignInManager<User> signInManager
            , IEmailService emailService
            , IConfiguration configuration,
            ImageService imageService
            ,IMapper mapper
            ,IGenericRepository<FavouriteList> favRepository)
        {
            this.userManager = userManager;
            this.tokenService = tokenService;
            this.signInManager = signInManager;
            this.emailService = emailService;
            this.configuration = configuration;
            this.imageService = imageService;
            this.mapper = mapper;
            this.favRepository = favRepository;
        }
        [HttpPost("Register")]
        public async Task<ActionResult<ReturnedDTO>> Register([FromForm]RegisterDTO registerDTO)
        {
            var user = new User()
            {
                Email = registerDTO.Email,
                FirstName = registerDTO.FirstName,
                LastName = registerDTO.SecondName,
                PhoneNumber = registerDTO.PhoneNumber,
                UserName = registerDTO.Email.Split('@')[0],
                Longitude = registerDTO.Longitude,
                Latitude = registerDTO.Latitude,
                Address = registerDTO.Address
            };
            if (registerDTO.File is not null)
            {
                user.Picture = await imageService.UploadImageAsync(registerDTO.File);
            }
            var result = await userManager.CreateAsync(user, registerDTO.Password);
            if (!result.Succeeded) return BadRequest("Invalid Entered Data");
            await SendEmail(user);
            var favouriteList = new FavouriteList() { UserId = user.Id };
            await favRepository.Add(favouriteList);
            var returnedUser = new ReturnedDTO()
            {
                DisplayName = registerDTO.FirstName,
                Email = registerDTO.Email,
                Token = await tokenService.GetTokenAsync(userManager, user),
            };
            return Ok(returnedUser);
        }
        [HttpPost("GoogleLogin")]
        public async Task<ActionResult<ReturnedDTO>> GoogleLogin(string googleToken)
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(googleToken);
            if (payload is null) return Unauthorized("Invalid Token");
            var user = await userManager.FindByEmailAsync(payload.Email);
            if (user is null)
            {
                user = new User()
                {
                    Email = payload.Email,
                    FirstName = payload.GivenName,
                    LastName = payload.FamilyName,
                    PhoneNumber = "00000000000",
                    UserName = payload.Email.Split('@')[0]
                };
                var result = await userManager.CreateAsync(user);
                if (!result.Succeeded) return BadRequest(result.Errors);
            }
            user.RefreshToken = tokenService.GenerateRefreshToken();
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            await signInManager.SignInAsync(user, false);
            if (user.VerifiedAt is null)
                user.VerifiedAt = DateTime.UtcNow;
            await userManager.UpdateAsync(user);
            var returnedUser = new ReturnedDTO()
            {
                DisplayName = user.FirstName,
                Email = user.Email,
                Token = await tokenService.GetTokenAsync(userManager, user),
                RefreshToken = user.RefreshToken,
                UserId = user.Id
            };
            return Ok(returnedUser);
        }
        [HttpGet("verifyaccount")]
        public async Task<ActionResult> VerifyAccount(string userId, string token)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user is null) return BadRequest("Invalid User");
            var confirmEmail = await userManager.ConfirmEmailAsync(user, token);
            if (confirmEmail.Succeeded)
            {
                user.VerifiedAt = DateTime.UtcNow;
                await userManager.UpdateAsync(user);
                return Ok("User Verfied Successfuly");
            }
            return BadRequest("User failed to be verified");
        }
        [HttpPost("login")]
        public async Task<ActionResult<ReturnedLogin>> login(LoginDto request)
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user is null) return Unauthorized("Invalid Email or Password");
            var confirmSignIn = await userManager.CheckPasswordAsync(user, request.Password);
            if (!confirmSignIn)
                return Unauthorized("Invalid Email or Password");
            if (user.VerifiedAt is null)
            {
                await SendEmail(user);
                return BadRequest("Please Verify your Email Address First to login , check your email");
            }
            var confirmLogin = await signInManager.PasswordSignInAsync(user, request.Password, request.RememberMe, false);
            var refreshToken = tokenService.GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = request.RememberMe ? DateTime.UtcNow.AddDays(30) : DateTime.UtcNow.AddDays(1);
            await userManager.UpdateAsync(user);
            var returnedUser = new ReturnedLogin()
            {
                AccessToken = await tokenService.GetTokenAsync(userManager, user),
                RefreshToken = refreshToken,
                DisplayName = user.FirstName,
                Email = request.Email,
                UserId = user.Id
            };
            return Ok(returnedUser);
        }
        [HttpPost("Refresh-Token")]
        public async Task<ActionResult> RefreshToken(RefreshTokenDTO refreshTokenDTO)
        {
            var principal = tokenService.GetClaimsPrincipalFromExpiredToken(refreshTokenDTO.AccessToken);
            if (principal is null)
                return BadRequest("Invalid access token or refresh token.");
            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await userManager.FindByIdAsync(userId);
            if (user is null)
                return BadRequest("Invalid User Token.");
            if (user.RefreshToken != refreshTokenDTO.RefreshToken)
                return BadRequest("Invalid Refresh Token.");
            if (user.RefreshTokenExpiry <= DateTime.UtcNow)
                return BadRequest("Refresh Token time Expired.");
            var newAccessToken = await tokenService.GetTokenAsync(userManager, user);
            var newRefreshToken = tokenService.GenerateRefreshToken();
            user.RefreshToken = newRefreshToken;
            // updatethis
            await userManager.UpdateAsync(user);
            return Ok(new
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                Email=user.Email,
                DisplayName=user.FirstName,
                userId=user.Id
            });
        }
        [HttpPost("forgetpassword")]
        public async Task<ActionResult> ForgetPassword([FromBody]string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user is null) return BadRequest("User doesn't exist");
            if (user.VerifiedAt is null)
            {
                await SendEmail(user);
                return BadRequest("Please Verify your Email Address First to login , check your email");
            }
            var otp = new Random().Next(1000, 9999);
            var expiry = DateTime.UtcNow.AddMinutes(10);
            user.ResetPasswordOTP = otp;
            user.ResetPasswordExpiry = expiry;
            await userManager.UpdateAsync(user);
            var body = $"Your OTP Code,Your OTP for password reset is: <b>{otp}</b>. It will expire in 10 minutes.";
            var emailDto = new EmailDto() {
                Body = body,
                Subject = "OTP TO Reset password for Sakenny Application",
                To = email
            };
            await emailService.SendEmail(emailDto);
            return Ok("An email was sent");
        }
        [HttpPost("verifyotp")]
        public async Task<ActionResult> VerifyOtp(OtpVerfyDto request)
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user is null || user.ResetPasswordExpiry < DateTime.UtcNow)
                return BadRequest("Invalid or expired OTP.");
            if (user.ResetPasswordOTP != request.Otp)
                return BadRequest("Incorrect OTP.");
            return Ok("OTP verified.");
        }
        [HttpPost("resetpassword")]
        public async Task<ActionResult> ResetPassword(ResetPasswordDto request)
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user == null || user.ResetPasswordExpiry < DateTime.UtcNow)
                return BadRequest("Invalid request.");

            var resetToken = await userManager.GeneratePasswordResetTokenAsync(user);
            var result = await userManager.ResetPasswordAsync(user, resetToken, request.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            // Clear OTP
            user.ResetPasswordOTP = null;
            user.ResetPasswordExpiry = null;
            await userManager.UpdateAsync(user);

            return Ok("Password reset successful.");
        }
        [HttpGet("GetUser")]
        [Authorize]
        public async Task<ActionResult<UserProfileReturnDto>> GetUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null) return BadRequest("Invalid User");
            var user = await userManager.FindByIdAsync(userId);
            if (user is null) return BadRequest("Invalid User");
            var userProfile = mapper.Map<UserProfileReturnDto>(user);
            return Ok(userProfile);
        }
        [HttpGet("GetUserById/{userId}")]
        public async Task<ActionResult<UserProfileReturnDto>> GetUser(string userId)
        {
            if (userId is null) return BadRequest("Invalid User");
            var user = await userManager.FindByIdAsync(userId);
            if (user is null) return BadRequest("Invalid User");
            var userProfile = mapper.Map<UserProfileReturnDto>(user);
            return Ok(userProfile);
        }
        [HttpPut("EditProfile")]
        [Authorize]
        public async Task<ActionResult> EditProfile([FromForm] UserProfileEditDto userProfileEditDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null) return BadRequest("Invalid User");
            var user = await userManager.FindByIdAsync(userId);
            if (user is null) return BadRequest("Invalid User");
            #region mappingUser
            user.Email = userProfileEditDto.Email;
            user.FirstName = userProfileEditDto.FirstName;
            user.LastName = userProfileEditDto.SecondName;
            user.PhoneNumber = userProfileEditDto.PhoneNumber;
            user.UserName = userProfileEditDto.Email.Split('@')[0];
            user.Longitude = userProfileEditDto.Longitude;
            user.Latitude = userProfileEditDto.Latitude;
            user.Address = userProfileEditDto.Address;
            #endregion
            if (userProfileEditDto.RemovedPicture)
            {
                await imageService.RemoveImageAsync(user.Picture);
                user.Picture = null;
            }
            if (userProfileEditDto.File is not null)
            {
                user.Picture = await imageService.UploadImageAsync(userProfileEditDto.File);
            }
            var result=await userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return BadRequest("Failed to update profile");
            var returnedUser = new
            {
                DisplayName = user.FirstName,
                Email = user.Email,
                AccessToken = await tokenService.GetTokenAsync(userManager, user),
                refreshToken = user.RefreshToken,
                userId=user.Id
            };
            return Ok(returnedUser);
        }
        private async Task SendEmail(User user)
        {
            var verificationToken = await userManager.GenerateEmailConfirmationTokenAsync(user);
            user.VerificationToken = verificationToken;
            var verificationLink = Url.Action("verifyaccount", "Account", new { userId = user.Id, token = verificationToken }, Request.Scheme);
            var emailBody = $"Please confirm your email by clicking the following link: <a href='{verificationLink}'>Confirm Email</a>";
            var emailDto = new EmailDto() { Body = emailBody, To = user.Email, Subject = "Confirmation Email" };
            await emailService.SendEmail(emailDto);
        }
    }
}
