using Portfolio_V2.Domain.Models;

namespace Portfolio_V2.Infrastructure.Repositories
{
    public interface IAditionalInfoRepository
    {
        Task<List<AditionalInfoItem>> ListAsync();
        Task<AditionalInfoItem?> GetAsync(Guid id);
        Task AddAsync(AditionalInfoItem item);
        Task UpdateAsync(AditionalInfoItem item);
        Task DeleteAsync(AditionalInfoItem item);
        Task SaveChangesAsync();
    }
}


