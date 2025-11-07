namespace Portfolio_V2.Contracts
{
    public record TestimonialResponse(Guid Id, string Name, string Highlight, DateTime CreatedAt, DateTime? UpdatedAt);

    public record CreateTestimonialRequest(string Name, string Highlight);

    public record UpdateTestimonialRequest(string Name, string Highlight);
}



