using Dapper;
using MySecureBackend.WebApi.Models;
using MySecureBackend.WebApi.Repositories;
using System.Data.SqlClient;

public class Environment2DRepository : IEnvironment2DRepository
{
    private readonly string _connectionString;

    public Environment2DRepository(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("DefaultConnection");
    }

    public async Task<IEnumerable<Environment2D>> GetAllByUserAsync(int userId)
    {
        using var connection = new SqlConnection(_connectionString);
        var sql = "SELECT * FROM Environment2D WHERE UserID = @UserID";
        return await connection.QueryAsync<Environment2D>(sql, new { UserID = userId });
    }

    public async Task<Environment2D?> GetByIdAsync(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        var sql = "SELECT * FROM Environment2D WHERE ID = @ID";
        return await connection.QueryFirstOrDefaultAsync<Environment2D>(sql, new { ID = id });
    }

    public async Task<Environment2D> CreateAsync(Environment2D env)
    {
        using var connection = new SqlConnection(_connectionString);
        var sql = @"INSERT INTO Environment2D (UserID, Name, MaxHeight, MaxLength)
                    VALUES (@UserID, @Name, @MaxHeight, @MaxLength);
                    SELECT CAST(SCOPE_IDENTITY() as int);";

        var id = await connection.ExecuteScalarAsync<int>(sql, env);
        env.ID = id;
        return env;
    }

    public async Task<bool> RenameAsync(int id, string newName)
    {
        using var connection = new SqlConnection(_connectionString);
        var sql = "UPDATE Environment2D SET Name = @Name WHERE ID = @ID";
        var rows = await connection.ExecuteAsync(sql, new { Name = newName, ID = id });
        return rows > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        var sql = "DELETE FROM Environment2D WHERE ID = @ID";
        var rows = await connection.ExecuteAsync(sql, new { ID = id });
        return rows > 0;
    }

    public async Task<int> CountByUserAsync(int userId)
    {
        using var connection = new SqlConnection(_connectionString);
        var sql = "SELECT COUNT(*) FROM Environment2D WHERE UserID = @UserID";
        return await connection.ExecuteScalarAsync<int>(sql, new { UserID = userId });
    }

    public async Task<bool> ExistsWithNameForUserAsync(int userId, string name)
    {
        using var connection = new SqlConnection(_connectionString);
        var sql = "SELECT COUNT(*) FROM Environment2D WHERE UserID = @UserID AND Name = @Name";
        var count = await connection.ExecuteScalarAsync<int>(sql, new { UserID = userId, Name = name });
        return count > 0;
    }
}
