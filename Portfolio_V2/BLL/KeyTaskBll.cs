using Portfolio_V2.Contracts;
using Portfolio_V2.Infrastructure.Repositories;
using System.Linq;

namespace Portfolio_V2.BLL
{
    public interface IKeyTaskBll
    {
        Task<List<KeyTaskResponse>> ListAsync(string lang);
        Task<KeyTaskResponse?> GetAsync(Guid id, string lang);
    }

    public class KeyTaskBll(IKeyTaskRepository repo, IKeyTaskTranslationRepository trRepo) : IKeyTaskBll
    {
        private readonly IKeyTaskRepository _repo = repo;
        private readonly IKeyTaskTranslationRepository _trRepo = trRepo;

        public async Task<List<KeyTaskResponse>> ListAsync(string lang)
        {
            var list = await _repo.ListAsync();
            if (lang != Language.English) return [.. list.Select(k => Map(k))];
            var results = new List<KeyTaskResponse>(list.Count);
            foreach (var k in list)
            {
                var tr = await _trRepo.GetByKeyTaskIdAsync(k.Id);
                results.Add(Map(k, tr));
            }
            return results;
        }

        public async Task<KeyTaskResponse?> GetAsync(Guid id, string lang)
        {
            var e = await _repo.GetAsync(id);
            if (e is null) return null;
            if (lang != Language.English) return Map(e);
            var tr = await _trRepo.GetByKeyTaskIdAsync(e.Id);
            return Map(e, tr);
        }

        private static KeyTaskResponse Map(Domain.Models.KeyTaskBullet e, Domain.Models.Translations.KeyTaskTranslation? tr = null)
        {
            return new KeyTaskResponse(
                e.Id,
                string.IsNullOrWhiteSpace(tr?.KeyTask) ? e.KeyTask : tr!.KeyTask,
                string.IsNullOrWhiteSpace(tr?.Description) ? e.description : tr!.Description,
                e.Technologies.Select(t => new KeyTaskTechnologyDto(t.Technology, t.TechnologyBadge)).ToArray()
            );
        }
    }
}


