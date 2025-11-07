using Portfolio_V2.Contracts;
using Portfolio_V2.Infrastructure.Repositories;

namespace Portfolio_V2.BLL
{
    public interface ITestimonialBll
    {
        Task<List<TestimonialResponse>> ListAsync(string lang);
        Task<TestimonialResponse?> GetAsync(Guid id, string lang);
    }

    public class TestimonialBll(ITestimonialRepository repo, ITestimonialTranslationRepository trRepo) : ITestimonialBll
    {
        private readonly ITestimonialRepository _repo = repo;
        private readonly ITestimonialTranslationRepository _trRepo = trRepo;

        public async Task<List<TestimonialResponse>> ListAsync(string lang)
        {
            var list = await _repo.ListAsync();
            if (lang != Language.English) return [.. list.Select(e => Map(e))];
            var results = new List<TestimonialResponse>(list.Count);
            foreach (var e in list)
            {
                var tr = await _trRepo.GetByTestimonialIdAsync(e.Id);
                results.Add(Map(e, tr));
            }
            return results;
        }

        public async Task<TestimonialResponse?> GetAsync(Guid id, string lang)
        {
            var e = await _repo.GetAsync(id);
            if (e is null) return null;
            if (lang != Language.English) return Map(e);
            var tr = await _trRepo.GetByTestimonialIdAsync(e.Id);
            return Map(e, tr);
        }

        private static TestimonialResponse Map(Domain.Models.Testimonial e, Domain.Models.Translations.TestimonialTranslation? tr = null)
        {
            return new TestimonialResponse(
                e.Id,
                string.IsNullOrWhiteSpace(tr?.Name) ? e.Name : tr!.Name!,
                string.IsNullOrWhiteSpace(tr?.Highlight) ? e.Highlight : tr!.Highlight!,
                e.CreatedAt,
                e.UpdatedAt
            );
        }
    }
}



