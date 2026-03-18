using MySecureBackend.WebApi.Models;

namespace MySecureBackend.WebApi.Repositories
{
    public interface IEnvironment2DRepository
    {
        Task<IEnumerable<Environment2D>> GetAllByUserAsync(int userId);
        Task<Environment2D?> GetByIdAsync(int id);
        Task<Environment2D> CreateAsync(Environment2D env);
        Task<bool> RenameAsync(int id, string newName);
        Task<bool> DeleteAsync(int id);

        Task<int> CountByUserAsync(int userId);
        Task<bool> ExistsWithNameForUserAsync(int userId, string name);
    }
}

