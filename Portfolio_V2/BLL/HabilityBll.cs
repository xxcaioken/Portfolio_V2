using Portfolio_V2.Contracts;
using Portfolio_V2.Infrastructure.Repositories;

namespace Portfolio_V2.BLL
{
    public interface IHabilityBll
    {
        Task<List<HabilityResponse>> ListAsync(string lang);
        Task<HabilityResponse?> GetAsync(Guid id, string lang);
    }

    public class HabilityBll(IHabilityRepository repo, IHabilityTranslationRepository trRepo) : IHabilityBll
    {
        private readonly IHabilityRepository _repo = repo;
        private readonly IHabilityTranslationRepository _trRepo = trRepo;

        public async Task<List<HabilityResponse>> ListAsync(string lang)
        {
            var list = await _repo.ListAsync();
            if (lang != Language.English)
                return [.. list.Select(h => Map(h))];

            var results = new List<HabilityResponse>(list.Count);
            foreach (var h in list)
            {
                var tr = await _trRepo.GetByHabilityIdAsync(h.Id);
                var bulletIds = h.Bullets.Select(b => b.Id).ToArray();
                var bTrs = bulletIds.Length > 0 ? await _trRepo.GetBulletTranslationsAsync(bulletIds) : Array.Empty<Domain.Models.Translations.HabilityBulletTranslation>();
                results.Add(Map(h, tr, bTrs));
            }
            return results;
        }

        public async Task<HabilityResponse?> GetAsync(Guid id, string lang)
        {
            var e = await _repo.GetAsync(id);
            if (e is null) return null;
            if (lang != Language.English) return Map(e);
            var tr = await _trRepo.GetByHabilityIdAsync(e.Id);
            var bTrs = e.Bullets.Count > 0 ? await _trRepo.GetBulletTranslationsAsync(e.Bullets.Select(b => b.Id).ToArray()) : Array.Empty<Domain.Models.Translations.HabilityBulletTranslation>();
            return Map(e, tr, bTrs);
        }

        private static HabilityResponse Map(Domain.Models.HabilityItem h, Domain.Models.Translations.HabilityItemTranslation? tr = null, Domain.Models.Translations.HabilityBulletTranslation[]? bTrs = null)
        {
            return new HabilityResponse(
                h.Id,
                string.IsNullOrWhiteSpace(tr?.Hability) ? h.Hability : tr!.Hability,
                h.Bullets.Select(b =>
                {
                    var bt = bTrs?.FirstOrDefault(x => x.HabilityBulletId == b.Id);
                    var text = string.IsNullOrWhiteSpace(bt?.Text) ? b.Text : bt!.Text;
                    return new HabilityBulletDto(b.Id, text, b.Badge);
                }).ToArray(),
                h.CreatedAt,
                h.UpdatedAt
            );
        }
    }
}


