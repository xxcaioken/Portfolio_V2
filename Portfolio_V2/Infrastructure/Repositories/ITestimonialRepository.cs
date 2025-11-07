using Portfolio_V2.Domain.Models;

namespace Portfolio_V2.Infrastructure.Repositories
{
    public interface ITestimonialRepository
    {
        Task<List<Testimonial>> ListAsync();
        Task<Testimonial?> GetAsync(Guid id);
        Task AddAsync(Testimonial item);
        Task UpdateAsync(Testimonial item);
        Task DeleteAsync(Testimonial item);
        Task SaveChangesAsync();
    }
}



