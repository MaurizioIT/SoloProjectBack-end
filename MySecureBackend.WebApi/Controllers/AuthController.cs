
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MySecureBackend.WebApi.Models;

namespace MySecureBackend.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly string _connectionString;
    private readonly JwtService _jwtService;

    public AuthController(IConfiguration configuration, JwtService jwtService)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
                            ?? configuration["SqlConnectionString"]
                            ?? throw new InvalidOperationException("No SQL connection string configured.");

        _jwtService = jwtService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginRequest([FromBody] Login data)
    {
        await using var connection = new SqlConnection(_connectionString);

        var user = await connection.QueryFirstOrDefaultAsync<dynamic>(
            "SELECT UserID AS Id, Username, Password FROM dbo.UnityUser WHERE Username = @Username",
            new { Username = data.Username }
        );
        if (user == null)
        {
            return BadRequest("Invalid username or password");
        }

        if (data.Password != user.Password)
        {
            return BadRequest("Invalid username or password");
        }

        var token = _jwtService.GenerateToken(user);

        return Ok(new
        {
            userId = user.Id,
            username = user.Username,
            token = token
        });
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterRequest([FromBody] Login data)
    {
        await using var connection = new SqlConnection(_connectionString);
        var result = await connection.ExecuteAsync(
            "INSERT INTO dbo.UnityUser (Username, Password) VALUES (@Username, @Password)",
            new { data.Username, data.Password });

        return Ok(new
        {
            message = "User registered"
        });
    }
}
