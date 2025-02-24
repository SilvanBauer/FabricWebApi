using FabricWebApi.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FabricWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly FabricWebApiDbContext _dbContext;

        public AuthController(IConfiguration configuration, FabricWebApiDbContext dbContext)
        {
            _configuration = configuration;
            _dbContext = dbContext;
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            var user = _dbContext.ApplicationUsers.FirstOrDefault(au => au.Username == model.Username && au.Password == model.Password.CreateSHA256Hash());
            if (user == null)
            {
                return Unauthorized("User not found or password invalid");
            }

            var administrators = _configuration.GetSection("Administrators").Get<string[]>() ?? Array.Empty<string>();
            var token = GenerateAccessToken(model.Username, administrators.Contains(model.Username));
            return Ok(new JwtSecurityTokenHandler().WriteToken(token));
        }

        [HttpPost("Register")]
        public IActionResult Register([FromBody] LoginModel model)
        {
            if (model.Password.Length < 6)
            {
                return Unauthorized("You need at least 6 characters in your password");
            }

            var userExistsAlready = _dbContext.ApplicationUsers.Any(au => au.Username == model.Username);
            if (userExistsAlready)
            {
                return Unauthorized("User cannot be created as it already exists");
            }

            var newUser = new ApplicationUser
            {
                Id = _dbContext.ApplicationUsers.Max(au => au.Id) + 1,
                Username = model.Username,
                Password = model.Password.CreateSHA256Hash()
            };
            _dbContext.ApplicationUsers.Add(newUser);
            _dbContext.SaveChanges();

            return Ok();
        }

        private JwtSecurityToken GenerateAccessToken(string userName, bool administrator)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, userName),
                new(ClaimTypes.Role, administrator ? Roles.Administrator : Roles.User)
            };

            // Create a JWT
            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"])),
                    SecurityAlgorithms.HmacSha256
                )
            );

            return token;
        }
    }
}
