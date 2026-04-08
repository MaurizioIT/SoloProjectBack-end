using MySecureBackend.WebApi.Models;

namespace MySecureBackend.WebApi.Repositories
{
    public interface IUserRepository
    {
        Task<User> InsertAsync(User user);
        Task DeleteAsync(int id);
        Task<IEnumerable<User>> SelectAsync();
        Task<User?> SelectAsync(int id);
        Task UpdateAsync(User user);
        Task<bool> VerifyCredentialsAsync(string username, string password);
    }
}