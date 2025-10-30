using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Portfolio_V2.Domain.Models;

namespace Portfolio_V2.Infrastructure.Repositories
{
    public interface IExperienceRepository
    {
        Task<List<ExperienceItem>> ListAsync();
        Task<ExperienceItem?> GetAsync(Guid id);
        Task AddAsync(ExperienceItem item);
        Task UpdateAsync(ExperienceItem item);
        Task DeleteAsync(ExperienceItem item);
        Task SaveChangesAsync();
    }
}


