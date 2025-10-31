using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio_V2.Contracts;
using Portfolio_V2.Domain.Models;
using Portfolio_V2.Infrastructure.Repositories;
using System.Security.Cryptography;

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
                return Ok(new AboutResponse("", "", "", "", "", "", "", null, "", null, Array.Empty<SocialLinkDto>()));
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

        [HttpPost("management/about/avatar")]
        [Authorize(Policy = "Admin")]
        [RequestSizeLimit(2 * 1024 * 1024 + 1024 * 100)]
        public async Task<ActionResult<AboutResponse>> UploadAvatar([FromForm] IFormFile file)
        {
            if (file is null || file.Length == 0) return BadRequest(new { error = "Arquivo ausente" });
            if (file.Length > 2 * 1024 * 1024) return BadRequest(new { error = "Arquivo muito grande" });
            var allowed = new[] { "image/png", "image/jpeg", "image/webp", "image/gif" };
            if (!allowed.Contains(file.ContentType)) return BadRequest(new { error = "Tipo não permitido" });

            using var sigStream = file.OpenReadStream();
            byte[] header = new byte[12];
            int _ = await sigStream.ReadAsync(header.AsMemory());
            sigStream.Position = 0;
            Span<byte> headerSpan = header.AsSpan();
            bool looksImage =
              (headerSpan.StartsWith(new byte[] { 0xFF, 0xD8 }) && file.ContentType == "image/jpeg") ||
              (headerSpan.StartsWith(new byte[] { 0x89, 0x50, 0x4E, 0x47 }) && file.ContentType == "image/png") ||
              (headerSpan.StartsWith("RIFF"u8) && file.ContentType == "image/webp") ||
              (headerSpan.StartsWith(new byte[] { 0x47, 0x49, 0x46, 0x38 }) && file.ContentType == "image/gif");
            if (!looksImage) return BadRequest(new { error = "Assinatura inválida" });

            var now = DateTime.UtcNow;
            var relDir = Path.Combine("uploads", now.Year.ToString("D4"), now.Month.ToString("D2"));
            var absDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", relDir);
            Directory.CreateDirectory(absDir);

            var ext = Path.GetExtension(file.FileName);
            if (string.IsNullOrWhiteSpace(ext))
                ext = file.ContentType switch { "image/png" => ".png", "image/jpeg" => ".jpg", "image/webp" => ".webp", "image/gif" => ".gif", _ => ".bin" };

            var name = Convert.ToHexString(RandomNumberGenerator.GetBytes(8)).ToLowerInvariant() + ext.ToLowerInvariant();
            var absPath = Path.Combine(absDir, name);
            using (var fs = System.IO.File.Create(absPath))
            {
                await file.CopyToAsync(fs);
            }
            var publicUrl = $"/{relDir.Replace("\\", "/")}/{name}";

            var existing = await _repo.GetAsync();
            var about = existing ?? new AboutInfo();
            about.AvatarUrl = publicUrl;
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
            a.AvatarUrl ?? string.Empty,
            a.FooterNote,
            a.Socials.Select(s => new SocialLinkDto(s.Label, s.Url, s.IconKey)).ToArray()
        );
    }
}


