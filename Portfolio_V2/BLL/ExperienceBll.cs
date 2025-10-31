using Portfolio_V2.Contracts;
using Portfolio_V2.Domain.Models;
using Portfolio_V2.Infrastructure.Repositories;

namespace Portfolio_V2.BLL
{
    public interface IExperienceBll
    {
        Task<List<ExperienceResponse>> ListAsync(string lang);
        Task<ExperienceResponse?> GetAsync(Guid id, string lang);
    }

    public class ExperienceBll(IExperienceRepository repo, IExperienceTranslationRepository trRepo) : IExperienceBll
    {
        private readonly IExperienceRepository _repo = repo;
        private readonly IExperienceTranslationRepository _trRepo = trRepo;

        public async Task<List<ExperienceResponse>> ListAsync(string lang)
        {
            var list = await _repo.ListAsync();
            if (lang != Language.Portuguese) return [.. list.Select(e => Map(e))];
            var results = new List<ExperienceResponse>(list.Count);
            foreach (var e in list)
            {
                var tr = await _trRepo.GetByExperienceIdAsync(e.Id);
                results.Add(Map(e, tr));
            }
            return results;
        }

        public async Task<ExperienceResponse?> GetAsync(Guid id, string lang)
        {
            var e = await _repo.GetAsync(id);
            if (e is null) return null;
            if (lang != Language.English) return Map(e);
            var tr = await _trRepo.GetByExperienceIdAsync(e.Id);
            return Map(e, tr);
        }

        private static ExperienceResponse Map(ExperienceItem e, Domain.Models.Translations.ExperienceItemTranslation? tr = null)
        {
            return new ExperienceResponse(
                e.Id,
                string.IsNullOrWhiteSpace(tr?.Company) ? e.Company : tr!.Company,
                string.IsNullOrWhiteSpace(tr?.Role) ? e.Role : tr!.Role,
                e.StartDate.ToString("yyyy-MM-dd"),
                e.EndDate?.ToString("yyyy-MM-dd"),
                tr?.Bullets?.Length > 0 ? tr!.Bullets : e.Bullets,
                e.CreatedAt,
                e.UpdatedAt
            );
        }
    }
}


