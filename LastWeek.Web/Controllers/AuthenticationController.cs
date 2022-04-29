using LastWeek.Web.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using LastWeek.Model;
using DataManager;
using LastWeek.Web.Model;

namespace LastWeek.Web.Controllers
{
    [ApiController]
    [Authorize]
    [Route("")]
    public class AuthenticationController : ControllerBase
    {
        public IServiceProvider ServiceProvider { get; set; }
        private UserManager userManager => ServiceProvider.GetService<UserManager>() ?? throw new Exception("Cannot run the application without user manager");
        private AppSettings appSettings;

        public AuthenticationController(IServiceProvider serviceProvider, IOptions<AppSettings> appSettings)
        {
            ServiceProvider = serviceProvider;
            this.appSettings = appSettings.Value;
        }

        [HttpGet("User")]
        public IActionResult GetUser()
        {
            return Ok(User);
        }

        [AllowAnonymous]
        [HttpPost("Signup")]
        public async Task<IActionResult> Register([FromBody] User newUserCreds)
        {
            using var userManager = this.userManager;

            if (!User.HasClaim(claim => claim.Type == ClaimTypes.Name))
            {
                newUserCreds.Role = UserRole.Basic;
            }

            var hash = new PasswordHasher<User>().HashPassword(newUserCreds, newUserCreds.Password);

            var addedUser = await userManager.UpsertUserAsync(newUserCreds.WithHashedPassword(hash));
            return addedUser == 1
                ? Ok(newUserCreds.WithoutPassword())
                : StatusCode(500);
        }

        [AllowAnonymous]
        [HttpPost("Signin")]
        public IActionResult Signin([FromBody] UserSigninInfos signinInfos)
        {
            using var userManager = this.userManager;
            var user = userManager.GetUsers().SingleOrDefault(u => u.Username == signinInfos.Username || signinInfos.IsUsernameEmail() && u.Email == signinInfos.Username);


            if (user == null)
            {
                return Unauthorized(SigninErrorType.UserNotFound);
            }

            var hashVerificationResult = new PasswordHasher<User>().VerifyHashedPassword(user, user?.Password ?? "", signinInfos.Password);
            if (hashVerificationResult == PasswordVerificationResult.Failed)
            {
                return Unauthorized(SigninErrorType.PasswordError);
            }
            else if(user!.Deleted)
            {
                return Unauthorized(SigninErrorType.AccountDisabled);
            }
            else
            {
                user.Token = GetToken(user);
                return Ok(user.WithoutPassword());
            }
        }

        private string GetToken(User user)
        {
            if(user.Deleted)
            {
                throw new ArgumentException("Cannot retrieve token for a deleted account");
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwt = appSettings.JwtSecret;
            if (jwt == null) throw new Exception("JWT secret required to run the application");

            var key = Encoding.ASCII.GetBytes(jwt);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Guid.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}