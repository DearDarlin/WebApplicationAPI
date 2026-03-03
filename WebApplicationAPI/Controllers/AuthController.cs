using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplicationAPI.CoreConfig;
using WebApplicationAPI.Models;

namespace WebApplicationAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly UsersStore _users;
        private readonly JwtToken _jwt;

        public AuthController(UsersStore users, IConfiguration config)
        {
            _users = users;
            _users.Create("admin", "admin", Roles.Admin);
            _jwt = new JwtToken(config);

        }

        [HttpPost("register")]
        [AllowAnonymous]
        public IActionResult Register(RegisterRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Email and password are required.");
            }

            var ok = _users.Create(request.Email, request.Password, Roles.User);

            return ok
                ? Ok()
                : Conflict("User with this email already exists.");
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Login(LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Email and password are required.");
            }
            var user = _users.Find(request.Email);

            if (!_users.CheckPassword(user, request.Password))
            {
                return Unauthorized();
            }
            return Ok(new AuthResponse { AccessToken = _jwt.Create(user) });
        }

        [HttpPost("createAdmin")]
        [Authorize(Roles=Roles.Admin)]
        public IActionResult CreateAdmin(RegisterRequest request)
        {
            var ok =_users.Create(request.Email, request.Password, Roles.Admin);
            return ok
                ? Ok()
                : Conflict("User with this email already exists.");

        }

    }
}
