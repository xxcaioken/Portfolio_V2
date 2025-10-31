using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Portfolio_V2.Domain.Models;

namespace Portfolio_V2.Infrastructure.Repositories
{
    public class AditionalInfoRepository(AppDbContext db) : IAditionalInfoRepository
    {
        private readonly AppDbContext _db = db;

        public async Task<List<AditionalInfoItem>> ListAsync()
        {
            return await _db.AditionalInfos.AsNoTracking()
                .Include(a => a.Bullets)
                .OrderByDescending(e => e.CreatedAt)
                .ToListAsync();
        }

        public async Task<AditionalInfoItem?> GetAsync(Guid id)
        {
            return await _db.AditionalInfos.Include(a => a.Bullets).FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task AddAsync(AditionalInfoItem item)
        {
            await _db.AditionalInfos.AddAsync(item);
        }

        public Task UpdateAsync(AditionalInfoItem item)
        {
            _db.AditionalInfos.Update(item);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(AditionalInfoItem item)
        {
            _db.AditionalInfos.Remove(item);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}


