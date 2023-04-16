using HRMSAPI.DTO;
using HRMSAPI.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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
        public IDataProtectionProvider _dataProtectionProvider { get; }

        public AccountController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager, IConfiguration appConfig, IDataProtectionProvider dataProtectionProvider)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _appConfig = appConfig;
            _dataProtectionProvider = dataProtectionProvider;
        }


        [HttpPost]
        public async Task<IActionResult> LogInAsync(LogInDTO loginDTO)
        {
            var issuer = _appConfig["JWT:Issuer"];
            var audience = _appConfig["JWT:Audience"];
            var key = _appConfig["JWT:Key"];

            // Log In
            if (ModelState.IsValid)
            {
                var credentials = await _signInManager.PasswordSignInAsync(loginDTO.UserName , loginDTO.Password, true, false);
                if (credentials.Succeeded)
                {
                    var user = await _signInManager.UserManager.FindByNameAsync(loginDTO.UserName);
                    if (user != null)
                    {
                        var roles = await _signInManager.UserManager.GetRolesAsync(user);

                        var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                        // Add the "Administrator" role claim if the user has the role
                        roles.Contains("Administrator") ? new Claim(ClaimTypes.Role, "Administrator") : null,
                        roles.Contains("Manager") ? new Claim(ClaimTypes.Role, "Manager") : null,
                        roles.Contains("Employee") ? new Claim(ClaimTypes.Role, "Employee") : null
                    };

                        var keyBytes = Encoding.UTF8.GetBytes(key);
                        var theKey = new SymmetricSecurityKey(keyBytes); // 256 bits of key
                        var creds = new SigningCredentials(theKey, SecurityAlgorithms.HmacSha256);
                        var token = new JwtSecurityToken(issuer, audience, claims.Where(x => x != null), expires: DateTime.Now.AddMinutes(30), signingCredentials: creds);
                        return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) }); // token 
                    }
                }
                return BadRequest("Invalid Credentialsss!");
            }
            return BadRequest(ModelState);
        }
    }
}
