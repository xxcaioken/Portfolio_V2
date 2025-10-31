using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio_V2.Contracts;
using Portfolio_V2.Domain.Models;
using Portfolio_V2.Infrastructure.Repositories;

namespace Portfolio_V2.Controllers
{
    [ApiController]
    [Route("")]
    public class AboutController(IAboutRepository repo) : ControllerBase
    {
        private readonly IAboutRepository _repo = repo;

        [HttpGet("about")]
        [AllowAnonymous]
        public async Task<ActionResult<AboutResponse>> Get()
        {
            var about = await _repo.GetAsync();
            if (about is null)
            {
                return Ok(new AboutResponse("", "", "", "", "", "", "", null, null, null, Array.Empty<SocialLinkDto>()));
            }
            return Ok(Map(about));
        }

        [HttpPut("management/about")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<AboutResponse>> Update([FromBody] UpdateAboutRequest req)
        {
            var existing = await _repo.GetAsync();
            var about = existing ?? new AboutInfo();
            about.Name = req.Name.Trim();
            about.Title = req.Title.Trim();
            about.Summary = req.Summary.Trim();
            about.Location = req.Location.Trim();
            about.Phone = req.Phone.Trim();
            about.Email = req.Email.Trim();
            about.Linkedin = req.Linkedin.Trim();
            about.Github = string.IsNullOrWhiteSpace(req.Github) ? null : req.Github.Trim();
            about.AvatarUrl = string.IsNullOrWhiteSpace(req.AvatarUrl) ? null : req.AvatarUrl.Trim();
            about.FooterNote = string.IsNullOrWhiteSpace(req.FooterNote) ? null : req.FooterNote.Trim();

            about.Socials.Clear();
            foreach (var s in req.Socials)
            {
                about.Socials.Add(new SocialLink { Label = s.Label.Trim(), Url = s.Url.Trim(), IconKey = s.IconKey?.Trim() });
            }

            await _repo.UpsertAsync(about);
            return Ok(Map(about));
        }

        private static AboutResponse Map(AboutInfo a) => new AboutResponse(
            a.Name,
            a.Title,
            a.Summary,
            a.Location,
            a.Phone,
            a.Email,
            a.Linkedin,
            a.Github,
            a.AvatarUrl,
            a.FooterNote,
            a.Socials.Select(s => new SocialLinkDto(s.Label, s.Url, s.IconKey)).ToArray()
        );
    }
}


