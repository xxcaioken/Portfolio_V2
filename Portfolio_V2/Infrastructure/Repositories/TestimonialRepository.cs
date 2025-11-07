using Microsoft.EntityFrameworkCore;
using Portfolio_V2.Domain.Models;

namespace Portfolio_V2.Infrastructure.Repositories
{
    public class TestimonialRepository(AppDbContext db) : ITestimonialRepository
    {
        private readonly AppDbContext _db = db;

        public async Task<List<Testimonial>> ListAsync()
        {
            return await _db.Testimonials.AsNoTracking()
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<Testimonial?> GetAsync(Guid id)
        {
            return await _db.Testimonials.FindAsync(id);
        }

        public async Task AddAsync(Testimonial item)
        {
            await _db.Testimonials.AddAsync(item);
        }

        public Task UpdateAsync(Testimonial item)
        {
            _db.Testimonials.Update(item);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Testimonial item)
        {
            _db.Testimonials.Remove(item);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}



