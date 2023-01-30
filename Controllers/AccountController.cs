using Api4.Models;
using Api4.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api4.Controllers
{
   // [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
           _signInManager = signInManager;
            _userManager = userManager;
        }


        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            var Success = "Account Created";
            var Failed = "Error";
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                return Ok(Success);
            }
            else
            {
                return Ok(Failed);
            }
        }


        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var Failed = "Login Failed";
            var user = await _userManager.FindByEmailAsync(model.Email);
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
            if (result.Succeeded)
            {
                var claim = new[]
                {
                    new Claim("Email",model.Email),
                    new Claim(ClaimTypes.NameIdentifier,user.Id)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("This is the security signing key"));
                var token = new JwtSecurityToken
                    (
                     issuer: "http://ahmadmozaffar.net",
                     audience: "http://ahmadmozaffar.net",
                     claims: claim,
                     expires:DateTime.Now.AddDays(10),
                     signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)) ;

                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                return Ok(tokenString);
            }
            else
            {
                return Ok(Failed);
            }

        }
    }
}
