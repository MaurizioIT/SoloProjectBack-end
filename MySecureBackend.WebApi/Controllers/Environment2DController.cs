using Microsoft.AspNetCore.Mvc;
using MySecureBackend.WebApi.Models;
using MySecureBackend.WebApi.Repositories;

[ApiController]
[Route("api/[controller]")]
public class Environment2DController : ControllerBase
{
    private readonly IEnvironment2DRepository _environmentRepo;
    private readonly IObject2DRepository _objectRepo;

    public Environment2DController(
        IEnvironment2DRepository environmentRepo,
        IObject2DRepository objectRepo)
    {
        _environmentRepo = environmentRepo;
        _objectRepo = objectRepo;
    }

    // GET api/environment2d?userId=1
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int userId)
    {
        if (userId <= 0)
            return BadRequest("Invalid userId.");

        var environments = await _environmentRepo.GetAllByUserAsync(userId);
        return Ok(environments);
    }

    // GET api/environment2d/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var env = await _environmentRepo.GetByIdAsync(id);
        if (env == null)
            return NotFound();

        return Ok(env);
    }

    // POST api/environment2d?userId=1
    [HttpPost]
    public async Task<IActionResult> Create([FromQuery] int userId, [FromBody] Environment2D environment)
    {
        if (userId <= 0)
            return BadRequest("Invalid userId.");

        if (environment == null)
            return BadRequest("Environment is required.");

        if (string.IsNullOrWhiteSpace(environment.Name) || environment.Name.Length > 25)
            return BadRequest("Name must be between 1 and 25 characters.");

        environment.UserID = userId;

        var count = await _environmentRepo.CountByUserAsync(userId);
        if (count >= 5)
            return BadRequest("You cannot have more than 5 worlds.");

        var exists = await _environmentRepo.ExistsWithNameForUserAsync(userId, environment.Name);
        if (exists)
            return BadRequest("You already have a world with this name.");

        var created = await _environmentRepo.CreateAsync(environment);
        return Ok(created);
    }

    // PUT api/environment2d/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Rename(int id, [FromBody] string newName)
    {
        if (string.IsNullOrWhiteSpace(newName) || newName.Length > 25)
            return BadRequest("Name must be between 1 and 25 characters.");

        var updated = await _environmentRepo.RenameAsync(id, newName);

        if (!updated)
            return NotFound();

        return Ok(new { message = "World renamed", newName });
    }

    // DELETE api/environment2d/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        // eerst alle objecten van deze wereld verwijderen
        await _objectRepo.DeleteByEnvironmentAsync(id);

        var deleted = await _environmentRepo.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return Ok(new { message = "World and its objects deleted" });
    }
}

