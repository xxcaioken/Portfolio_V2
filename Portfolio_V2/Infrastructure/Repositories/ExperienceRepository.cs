using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Portfolio_V2.Domain.Models;

namespace Portfolio_V2.Infrastructure.Repositories
{
    public class ExperienceRepository(AppDbContext db) : IExperienceRepository
    {
        private readonly AppDbContext _db = db;

        public async Task<List<ExperienceItem>> ListAsync()
        {
            return await _db.Experiences.AsNoTracking()
                .OrderByDescending(e => e.CreatedAt)
                .ToListAsync();
        }

        public async Task<ExperienceItem?> GetAsync(Guid id)
        {
            return await _db.Experiences.FindAsync(id);
        }

        public async Task AddAsync(ExperienceItem item)
        {
            await _db.Experiences.AddAsync(item);
        }

        public Task UpdateAsync(ExperienceItem item)
        {
            _db.Experiences.Update(item);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(ExperienceItem item)
        {
            _db.Experiences.Remove(item);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}


