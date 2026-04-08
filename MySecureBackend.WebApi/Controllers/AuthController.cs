using Microsoft.AspNetCore.Mvc;
using MySecureBackend.WebApi.Models;
using MySecureBackend.WebApi.Repositories;

namespace MySecureBackend.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public AuthController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Verifies login credentials and returns whether the login is valid.
        /// </summary>
        /// <param name="loginRequest">The login credentials (username and password)</param>
        /// <returns>Returns true if credentials are valid, false otherwise</returns>
        [HttpPost("login", Name = "VerifyLogin")]
        public async Task<ActionResult<bool>> Login([FromBody] Login loginRequest)
        {
            if (loginRequest == null || string.IsNullOrWhiteSpace(loginRequest.Username) || string.IsNullOrWhiteSpace(loginRequest.Password))
                return BadRequest(new ProblemDetails { Detail = "Username and password are required" });

            // Get all users and find matching username
            var users = await _userRepository.SelectAsync();
            var user = users.FirstOrDefault(u => u.Username == loginRequest.Username);

            if (user == null)
                return Ok(false); // User not found

            // Verify password (in production, use proper hashing verification)
            bool isPasswordValid = user.Password == loginRequest.Password;

            return Ok(isPasswordValid);
        }
    }
}
