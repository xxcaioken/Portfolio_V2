using Portfolio_V2.Contracts;
using Portfolio_V2.Infrastructure.Repositories;

namespace Portfolio_V2.BLL
{
    public interface IRecommendationLetterBll
    {
        Task<List<RecommendationLetterResponse>> ListAsync();
        Task<RecommendationLetterResponse?> GetAsync(Guid id);
    }

    public class RecommendationLetterBll(IRecommendationLetterRepository repo) : IRecommendationLetterBll
    {
        private readonly IRecommendationLetterRepository _repo = repo;

        public async Task<List<RecommendationLetterResponse>> ListAsync()
        {
            var list = await _repo.ListAsync();
            return [.. list.Select(Map)];
        }

        public async Task<RecommendationLetterResponse?> GetAsync(Guid id)
        {
            var e = await _repo.GetAsync(id);
            return e is null ? null : Map(e);
        }

        private static RecommendationLetterResponse Map(Domain.Models.RecommendationLetter r)
            => new(r.Id, r.ImageUrlPt, r.ImageUrlEn, r.CreatedAt, r.UpdatedAt);
    }
}


