using Portfolio_V2.Contracts;
using Portfolio_V2.Infrastructure.Repositories;

namespace Portfolio_V2.BLL
{
    public interface IAditionalInfoBll
    {
        Task<List<AditionalInfoResponse>> ListAsync(string lang);
        Task<AditionalInfoResponse?> GetAsync(Guid id, string lang);
    }

    public class AditionalInfoBll(IAditionalInfoRepository repo, IAditionalInfoTranslationRepository trRepo) : IAditionalInfoBll
    {
        private readonly IAditionalInfoRepository _repo = repo;
        private readonly IAditionalInfoTranslationRepository _trRepo = trRepo;

        public async Task<List<AditionalInfoResponse>> ListAsync(string lang)
        {
            var list = await _repo.ListAsync();
            if (lang != Language.English)
                return [.. list.Select(h => Map(h))];

            var results = new List<AditionalInfoResponse>(list.Count);
            foreach (var h in list)
            {
                var tr = await _trRepo.GetByAditionalInfoIdAsync(h.Id);
                var bulletIds = h.Bullets.Select(b => b.Id).ToArray();
                var bTrs = bulletIds.Length > 0 ? await _trRepo.GetBulletTranslationsAsync(bulletIds) : Array.Empty<Domain.Models.Translations.AditionalInfoBulletTranslation>();
                results.Add(Map(h, tr, bTrs));
            }
            return results;
        }

        public async Task<AditionalInfoResponse?> GetAsync(Guid id, string lang)
        {
            var e = await _repo.GetAsync(id);
            if (e is null) return null;
            if (lang != Language.English) return Map(e);
            var tr = await _trRepo.GetByAditionalInfoIdAsync(e.Id);
            var bTrs = e.Bullets.Count > 0 ? await _trRepo.GetBulletTranslationsAsync(e.Bullets.Select(b => b.Id).ToArray()) : Array.Empty<Domain.Models.Translations.AditionalInfoBulletTranslation>();
            return Map(e, tr, bTrs);
        }

        private static AditionalInfoResponse Map(Domain.Models.AditionalInfoItem h, Domain.Models.Translations.AditionalInfoItemTranslation? tr = null, Domain.Models.Translations.AditionalInfoBulletTranslation[]? bTrs = null)
        {
            return new AditionalInfoResponse(
                h.Id,
                string.IsNullOrWhiteSpace(tr?.AditionalInfo) ? h.AditionalInfo : tr!.AditionalInfo,
                h.Bullets.Select(b =>
                {
                    var bt = bTrs?.FirstOrDefault(x => x.AditionalInfoBulletId == b.Id);
                    var text = string.IsNullOrWhiteSpace(bt?.Text) ? b.Text : bt!.Text;
                    return new AditionalInfoBulletDto(b.Id, text, b.Level, b.StartDate?.ToString("yyyy-MM-dd"), b.EndDate?.ToString("yyyy-MM-dd"));
                }).ToArray(),
                h.CreatedAt,
                h.UpdatedAt
            );
        }
    }
}


