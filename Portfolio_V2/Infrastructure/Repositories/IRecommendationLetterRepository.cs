using Portfolio_V2.Domain.Models;

namespace Portfolio_V2.Infrastructure.Repositories
{
    public interface IRecommendationLetterRepository
    {
        Task<List<RecommendationLetter>> ListAsync();
        Task<RecommendationLetter?> GetAsync(Guid id);
        Task AddAsync(RecommendationLetter item);
        Task UpdateAsync(RecommendationLetter item);
        Task DeleteAsync(RecommendationLetter item);
        Task SaveChangesAsync();
    }
}


