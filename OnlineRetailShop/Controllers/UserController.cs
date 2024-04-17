using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OnlineRetailShop.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OnlineRetailShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public IConfiguration _configuration;
        public readonly CommonContext _context;

        public UserController(IConfiguration configuration, CommonContext context)
        {
            _configuration = configuration;
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser(Credentials user)
        {
            if (user != null && user.Role != null && user.Password != null)
            {
                var userData = await GetUser(user.Role, user.Password);
                if (userData == null) { return BadRequest("Invalid Credentials"); }
                if (user.Role == userData.Role && user.Password == userData.Password)
                {
                    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                    var claims = new[] {
                            new Claim(JwtRegisteredClaimNames.Sub, user.Role),
                            new Claim("ID",user.Id.ToString()),
                            new Claim("Password",user.Password),
                            new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
                    };

                    var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                        _configuration["Jwt:Issuer"],
                        claims,
                        expires: DateTime.Now.AddMinutes(120),
                        signingCredentials: credentials);

                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));

                }
            }
            return BadRequest("Invalid Credentials");
        }

        [HttpGet]
        public async Task<Credentials> GetUser(string username, string password)
        {
            Credentials user = null;
            user = _context.Credentials.FirstOrDefault(x => x.Role == username);
            if (user.Password != password) return null;
            return user;
        }
    }
}
