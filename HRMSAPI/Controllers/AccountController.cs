using HRMSAPI.DTO;
using HRMSAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace HRMSAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private SignInManager<ApplicationUser> _signInManager;
        public IConfiguration _appConfig { get; }

        public AccountController( UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager, IConfiguration appConfig )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _appConfig = appConfig;
        }

        [HttpPost]
        public async Task<IActionResult> LogInAsync(LogInDTO loginDTO)
        {
            var issuer = _appConfig["JWT:Issuer"];
            var audience = _appConfig["JWT:Audience"];
            var key = _appConfig["JWT:Key"];
            
        if (ModelState.IsValid)
        {
            var credentials = await _signInManager.PasswordSignInAsync(loginDTO.UserName, loginDTO.Password, true, false);
            if (credentials.Succeeded)
            {
                var user = _signInManager.GetExternalLoginInfoAsync(loginDTO.UserName);
                if (user != null)
                {
                    var keyBytes = Encoding.UTF8.GetBytes(key);
                    var theKey = new SymmetricSecurityKey(keyBytes); // 256 bits of key
                    var creds = new SigningCredentials(theKey, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(issuer, audience, null, expires: DateTime.Now.AddMinutes(30), signingCredentials: creds);
                    return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) }); // token 
                }
            }
            return BadRequest("Invalid Credentials!");
        }
        return BadRequest(ModelState);
        }
    }
}
