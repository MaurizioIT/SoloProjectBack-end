using Dapper;
using MySecureBackend.WebApi.Models;
using MySecureBackend.WebApi.Repositories;
using System.Data.SqlClient;

public class Object2DRepository : IObject2DRepository
{
    private readonly string _connectionString;

    public Object2DRepository(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("DefaultConnection");
    }

    public async Task<IEnumerable<Object2D>> GetByEnvironmentAsync(int environmentId)
    {
        using var connection = new SqlConnection(_connectionString);
        var sql = @"SELECT * FROM Object2D WHERE EnvironmentID = @EnvironmentID";
        return await connection.QueryAsync<Object2D>(sql, new { EnvironmentID = environmentId });
    }

    public async Task<Object2D> CreateAsync(Object2D obj)
    {
        using var connection = new SqlConnection(_connectionString);

        var sql = @"
            INSERT INTO Object2D 
            (EnvironmentID, PrefabID, PositionX, PositionY, ScaleX, ScaleY, RotationZ, SortingLayer)
            VALUES 
            (@EnvironmentID, @PrefabID, @PositionX, @PositionY, @ScaleX, @ScaleY, @RotationZ, @SortingLayer);
            SELECT CAST(SCOPE_IDENTITY() as int);";

        var id = await connection.ExecuteScalarAsync<int>(sql, obj);
        obj.ID = id;
        return obj;
    }

    public async Task<bool> DeleteByEnvironmentAsync(int environmentId)
    {
        using var connection = new SqlConnection(_connectionString);
        var sql = @"DELETE FROM Object2D WHERE EnvironmentID = @EnvironmentID";
        var rows = await connection.ExecuteAsync(sql, new { EnvironmentID = environmentId });
        return rows > 0;
    }
}

