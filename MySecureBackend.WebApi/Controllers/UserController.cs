using Microsoft.AspNetCore.Mvc;
using MySecureBackend.WebApi.Models;
using MySecureBackend.WebApi.Repositories;
using MySecureBackend.WebApi.Services;

namespace MySecureBackend.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
[Consumes("application/json")]
[Produces("application/json")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IAuthenticationService _authenticationService;

    public UserController(IUserRepository userRepository, IAuthenticationService authenticationService)
    {
        _userRepository = userRepository;
        _authenticationService = authenticationService;
    }

    [HttpGet(Name = "GetUsers")]
    public async Task<ActionResult<List<User>>> GetAsync()
    {
        var user = await _userRepository.SelectAsync();
        return Ok(user);
    }

    [HttpGet("{userId}", Name = "GetUserById")]
    public async Task<ActionResult<User>> GetByIdAsync(int userId)
    {
        var user = await _userRepository.SelectAsync(userId);

        if (user == null)
            return NotFound(new ProblemDetails { Detail = $"User {userId} not found" });

        return Ok(user);
    }

    [HttpPost(Name = "AddUser")]
    public async Task<ActionResult<User>> AddAsync(User user)
    {
        //exampleObject.Id = Guid.NewGuid();

        var createdUser = await _userRepository.InsertAsync(user);

        return CreatedAtRoute("GetUserById", new { userId = createdUser.UserID }, createdUser);
    }

    [HttpPut("{userId}", Name = "UpdateUser")]
    public async Task<ActionResult<User>> UpdateAsync(int userId, User user)
    {
        var existingUser = await _userRepository.SelectAsync(userId);

        if (existingUser == null)
            return NotFound(new ProblemDetails { Detail = $"User {userId} not found" });

        if (user.UserID != userId)
            return Conflict(new ProblemDetails { Detail = "The id of the User in the route does not match the id of the User in the body" });

        await _userRepository.UpdateAsync(user);

        return Ok(user);
    }

    [HttpDelete("{userId}", Name = "DeleteUser")]
    public async Task<ActionResult> DeleteAsync(int userId)
    {
        var exampleObject = await _userRepository.SelectAsync(userId);

        if (exampleObject == null)
            return NotFound(new ProblemDetails { Detail = $"User {userId} not found" });

        await _userRepository.DeleteAsync(userId);

        return Ok();
    }
}