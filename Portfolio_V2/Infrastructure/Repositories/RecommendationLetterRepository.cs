using Microsoft.EntityFrameworkCore;
using Portfolio_V2.Domain.Models;

namespace Portfolio_V2.Infrastructure.Repositories
{
    public class RecommendationLetterRepository(AppDbContext db) : IRecommendationLetterRepository
    {
        private readonly AppDbContext _db = db;

        public async Task<List<RecommendationLetter>> ListAsync()
        {
            return await _db.RecommendationLetters.AsNoTracking()
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<RecommendationLetter?> GetAsync(Guid id)
        {
            return await _db.RecommendationLetters.FindAsync(id);
        }

        public async Task AddAsync(RecommendationLetter item)
        {
            await _db.RecommendationLetters.AddAsync(item);
        }

        public Task UpdateAsync(RecommendationLetter item)
        {
            _db.RecommendationLetters.Update(item);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(RecommendationLetter item)
        {
            _db.RecommendationLetters.Remove(item);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}


