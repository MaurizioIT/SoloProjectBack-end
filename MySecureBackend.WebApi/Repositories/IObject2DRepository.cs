using MySecureBackend.WebApi.Models;

namespace MySecureBackend.WebApi.Repositories
{
    public interface IObject2DRepository
    {
        Task<IEnumerable<Object2D>> GetByEnvironmentAsync(int environmentId);
        Task<Object2D> CreateAsync(Object2D obj);
        Task<bool> DeleteByEnvironmentAsync(int environmentId);
    }
}