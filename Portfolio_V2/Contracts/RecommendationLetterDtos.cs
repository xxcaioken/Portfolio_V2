namespace Portfolio_V2.Contracts
{
    public record RecommendationLetterResponse(Guid Id, string? ImageUrlPt, string? ImageUrlEn, DateTime CreatedAt, DateTime? UpdatedAt);

    public record CreateRecommendationLetterRequest(string? ImageUrlPt, string? ImageUrlEn);

    public record UpdateRecommendationLetterRequest(string? ImageUrlPt, string? ImageUrlEn);
}


