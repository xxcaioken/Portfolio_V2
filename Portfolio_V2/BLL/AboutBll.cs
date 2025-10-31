using Portfolio_V2.Contracts;
using Portfolio_V2.Domain.Models;
using Portfolio_V2.Domain.Models.Translations;
using Portfolio_V2.Infrastructure.Repositories;

namespace Portfolio_V2.BLL
{
    public interface IAboutBll
    {
        Task<AboutResponse> GetAsync(string lang);
    }

    public class AboutBll(IAboutRepository aboutRepo, IAboutTranslationRepository trRepo) : IAboutBll
    {
        private readonly IAboutRepository _aboutRepo = aboutRepo;
        private readonly IAboutTranslationRepository _trRepo = trRepo;

        public async Task<AboutResponse> GetAsync(string lang)
        {
            var about = await _aboutRepo.GetAsync();
            if (about is null)
            {
                return new AboutResponse("", "", "", "", "", "", "", null, "", null, Array.Empty<SocialLinkDto>());
            }
            if (lang == Language.English)
            {
                var tr = await _trRepo.GetByAboutIdAsync(about.Id);
                if (tr is not null)
                {
                    return new AboutResponse(
                        string.IsNullOrWhiteSpace(tr.Name) ? about.Name : tr.Name,
                        string.IsNullOrWhiteSpace(tr.Title) ? about.Title : tr.Title,
                        string.IsNullOrWhiteSpace(tr.Summary) ? about.Summary : tr.Summary,
                        string.IsNullOrWhiteSpace(tr.Location) ? about.Location : tr.Location,
                        about.Phone,
                        about.Email,
                        about.Linkedin,
                        about.Github,
                        about.AvatarUrl ?? string.Empty,
                        string.IsNullOrWhiteSpace(tr.FooterNote) ? about.FooterNote : tr.FooterNote,
                        about.Socials.Select(s => new SocialLinkDto(
                            tr.Socials.FirstOrDefault(x => x.Url == s.Url)?.Label ?? s.Label,
                            s.Url,
                            s.IconKey
                        )).ToArray()
                    );
                }
            }
            return new AboutResponse(
                about.Name,
                about.Title,
                about.Summary,
                about.Location,
                about.Phone,
                about.Email,
                about.Linkedin,
                about.Github,
                about.AvatarUrl ?? string.Empty,
                about.FooterNote,
                about.Socials.Select(s => new SocialLinkDto(s.Label, s.Url, s.IconKey)).ToArray()
            );
        }
    }
}


