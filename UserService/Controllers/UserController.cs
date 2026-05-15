using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using UserService.Models;
using UserService.Services;

namespace UserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IAuthService _authService;

        public UserController(IAuthService authService)
        {
            _authService = authService;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] LoginModal req)
        {
            if (string.IsNullOrWhiteSpace(req.Email) || string.IsNullOrWhiteSpace(req.Password) || string.IsNullOrWhiteSpace(req.Email))
                return BadRequest("Username, email and password are required");

            var (success, error) = await _authService.RegisterAsync(req);

            if (!success)
                return BadRequest(error);
            return Ok(new { message = "Registered" });
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.Username) || string.IsNullOrWhiteSpace(req.Password))
                return BadRequest("Username and password are required");

            var (success, token, error) = await _authService.LoginAsync(req.Username, req.Password);
            if (!success)
                return Unauthorized(new { error });

            return Ok(new { token });
        }

     

    }


    public record RegisterRequest(LoginModal LoginModal);
    public record LoginRequest(string Username, string Password);
}
