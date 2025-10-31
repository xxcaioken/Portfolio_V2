using Portfolio_V2.Domain.Models;

namespace Portfolio_V2.Infrastructure.Repositories
{
    public interface IAboutRepository
    {
        Task<AboutInfo?> GetAsync();
        Task UpsertAsync(AboutInfo about);
    }
}


