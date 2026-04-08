using Dapper;
using Microsoft.Data.SqlClient;
using MySecureBackend.WebApi.Models;

namespace MySecureBackend.WebApi.Repositories
{
    public class SqlUserRepository : IUserRepository
    {
        private readonly string sqlConnectionString;

        public SqlUserRepository(string sqlConnectionString)
        {
            this.sqlConnectionString = sqlConnectionString;
        }

        public async Task<User> InsertAsync(User user)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                var id = await sqlConnection.ExecuteScalarAsync<int>(
                    "INSERT INTO Users (Name, Password) OUTPUT INSERTED.ID VALUES (@Username, @Password)",
                    user);
                user.UserID = id;
                return user;
            }
        }

        public async Task<User?> SelectAsync(int id)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                return await sqlConnection.QuerySingleOrDefaultAsync<User>("SELECT * FROM Users WHERE Id = @Id", new { id });
            }
        }

        public async Task<IEnumerable<User>> SelectAsync()
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                return await sqlConnection.QueryAsync<User>("SELECT * FROM Users");
            }
        }

        public async Task UpdateAsync(User user)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync("UPDATE Users SET " +
                                                 "Name = @Username, " +
                                                 "Password = @Password " +
                                                 "WHERE Id = @UserID", user);

            }
        }

        public async Task DeleteAsync(int id)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync("DELETE FROM users WHERE Id = @Id", new { id });
            }
        }
    }
}
