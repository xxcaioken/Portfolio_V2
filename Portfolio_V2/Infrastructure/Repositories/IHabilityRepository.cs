using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Portfolio_V2.Domain.Models;

namespace Portfolio_V2.Infrastructure.Repositories
{
    public interface IHabilityRepository
    {
        Task<List<HabilityItem>> ListAsync();
        Task<HabilityItem?> GetAsync(Guid id);
        Task AddAsync(HabilityItem item);
        Task UpdateAsync(HabilityItem item);
        Task DeleteAsync(HabilityItem item);
        Task SaveChangesAsync();
    }
}


