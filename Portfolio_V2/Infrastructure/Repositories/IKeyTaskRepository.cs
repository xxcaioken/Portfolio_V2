using Portfolio_V2.Domain.Models;

namespace Portfolio_V2.Infrastructure.Repositories
{
    public interface IKeyTaskRepository
    {
        Task<List<KeyTaskBullet>> ListAsync();
        Task<KeyTaskBullet?> GetAsync(Guid id);
        Task AddAsync(KeyTaskBullet item);
        Task UpdateAsync(KeyTaskBullet item);
        Task DeleteAsync(KeyTaskBullet item);
        Task SaveChangesAsync();
    }
}


