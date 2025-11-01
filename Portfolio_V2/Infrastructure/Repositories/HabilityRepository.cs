using Microsoft.EntityFrameworkCore;
using Portfolio_V2.Domain.Models;

namespace Portfolio_V2.Infrastructure.Repositories
{
    public class HabilityRepository(AppDbContext db) : IHabilityRepository
    {
        private readonly AppDbContext _db = db;

        public async Task<List<HabilityItem>> ListAsync()
        {
            return await _db.Habilities.AsNoTracking()
                .Include(h => h.Bullets)
                .OrderByDescending(e => e.CreatedAt)
                .ToListAsync();
        }

        public async Task<HabilityItem?> GetAsync(Guid id)
        {
            return await _db.Habilities.Include(h => h.Bullets).FirstOrDefaultAsync(h => h.Id == id);
        }

        public async Task AddAsync(HabilityItem item)
        {
            await _db.Habilities.AddAsync(item);
        }

        public Task UpdateAsync(HabilityItem item)
        {
            _db.Habilities.Update(item);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(HabilityItem item)
        {
            _db.Habilities.Remove(item);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}


