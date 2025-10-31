using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace Portfolio_V2.Controllers
{
    [ApiController]
    [Route("")]
    public class UploadController : ControllerBase
    {
        private static readonly string[] AllowedContentTypes = { "image/png", "image/jpeg", "image/webp", "image/gif" };
        private const long MaxBytes = 2 * 1024 * 1024; // 2MB

        [HttpPost("management/uploads/image")]
        [Authorize(Policy = "Admin")]
        [RequestSizeLimit(MaxBytes + 1024 * 100)]
        public async Task<ActionResult<object>> UploadImage([FromForm] IFormFile file)
        {
            if (file is null || file.Length == 0) return BadRequest(new { error = "Arquivo ausente" });
            if (file.Length > MaxBytes) return BadRequest(new { error = "Arquivo muito grande" });
            if (!AllowedContentTypes.Contains(file.ContentType)) return BadRequest(new { error = "Tipo não permitido" });

            using var sigStream = file.OpenReadStream();
            byte[] header = new byte[12];
            int read = await sigStream.ReadAsync(header.AsMemory());
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
            return Ok(new { url = publicUrl });
        }
    }
}


