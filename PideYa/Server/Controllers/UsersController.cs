using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PideYa.Server.Dtos;
using PideYa.Server.Helpers;
using PideYa.Shared.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PideYa.Server.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly DataContext dataContext;
        private readonly AppSettings appSettings;

        public UsersController(DataContext dataContext, IOptions<AppSettings> appSettings)
        {
            this.dataContext = dataContext;
            this.appSettings = appSettings.Value;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAll()
        {
            var users = await dataContext.Users.ToListAsync();
            return Ok(users);
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] AuthenticateRequest model)
        {
            var response = authenticate(model);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(response);
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterRequest model)
        {
            var user = new Shared.Entities.User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Title = $"{model.FirstName}, {model.LastName}",
                Email = model.Email,
                PasswordHash = model.Password,
                Role = Role.Diner
            };
            dataContext.Users.Add(user);

            dataContext.SaveChanges();

            var response = new AuthenticateResponse(user, generateJwtToken(user));

            return Ok(response);
        }

        private AuthenticateResponse? authenticate(AuthenticateRequest request)
        {
            var user = dataContext.Users.FirstOrDefault(u => u.Email == request.Username && u.PasswordHash == request.Password);
            if (user is null) return null;
            var token = generateJwtToken(user);

            return new AuthenticateResponse(user, token);
        }

        private string generateJwtToken(User user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}

