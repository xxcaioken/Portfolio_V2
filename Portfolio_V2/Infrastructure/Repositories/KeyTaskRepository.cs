using Microsoft.EntityFrameworkCore;
using Portfolio_V2.Domain.Models;

namespace Portfolio_V2.Infrastructure.Repositories
{
    public class KeyTaskRepository(AppDbContext db) : IKeyTaskRepository
    {
        private readonly AppDbContext _db = db;

        public async Task<List<KeyTaskBullet>> ListAsync()
        {
            return await _db.Set<KeyTaskBullet>().AsNoTracking().Include(k => k.Technologies).OrderByDescending(k => k.Id).ToListAsync();
        }

        public async Task<KeyTaskBullet?> GetAsync(Guid id)
        {
            return await _db.Set<KeyTaskBullet>().Include(k => k.Technologies).FirstOrDefaultAsync(k => k.Id == id);
        }

        public async Task AddAsync(KeyTaskBullet item)
        {
            await _db.Set<KeyTaskBullet>().AddAsync(item);
        }

        public Task UpdateAsync(KeyTaskBullet item)
        {
            _db.Set<KeyTaskBullet>().Update(item);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(KeyTaskBullet item)
        {
            _db.Set<KeyTaskBullet>().Remove(item);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}


