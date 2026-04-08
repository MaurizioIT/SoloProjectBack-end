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
            if (loginRequest == null || string.IsNullOrWhiteSpace(loginRequest.username) || string.IsNullOrWhiteSpace(loginRequest.password))
                return BadRequest(new ProblemDetails { Detail = "Username and password are required" });

            // Verify credentials directly from database
            bool isValid = await _userRepository.VerifyCredentialsAsync(loginRequest.username, loginRequest.password);

            return Ok(isValid);
        }
    }
}
