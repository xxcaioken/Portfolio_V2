using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio_V2.Contracts;
using Portfolio_V2.Domain.Models;
using Portfolio_V2.Infrastructure.Repositories;
using System.Reflection.Emit;

namespace Portfolio_V2.Controllers
{
    [ApiController]
    [Route("")]
    public class AditionalInfoController(IAditionalInfoRepository repo) : ControllerBase
    {
        private readonly IAditionalInfoRepository _repo = repo;

        [HttpGet("AditionalInfos")]
        [AllowAnonymous]
        public async Task<ActionResult<List<AditionalInfoResponse>>> List()
        {
            List<AditionalInfoItem> list = await _repo.ListAsync();
            return Ok(list.Select(MapResponse));
        }

        [HttpGet("AditionalInfos/{id:guid}")]
        [AllowAnonymous]
        public async Task<ActionResult<AditionalInfoResponse>> Get(Guid id)
        {
            AditionalInfoItem? e = await _repo.GetAsync(id);
            if (e is null) return NotFound();
            return Ok(MapResponse(e));
        }

        [HttpPost("management/AditionalInfos")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<AditionalInfoResponse>> Create([FromBody] CreateAditionalInfoRequest req)
        {
            AditionalInfoItem e = GetE(req);
            await _repo.AddAsync(e);
            await _repo.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = e.Id }, MapResponse(e));
        }

        private static AditionalInfoItem GetE(CreateAditionalInfoRequest req)
        {
            var item = new AditionalInfoItem
            {
                AditionalInfo = req.AditionalInfo.Trim(),
                Bullets = new List<AditionalInfoBullet>(),
            };
            foreach (var b in req.Bullets ?? Array.Empty<AditionalInfoBulletDto>())
            {
                item.Bullets.Add(new AditionalInfoBullet
                {
                    Text = b.Text.Trim(),
                    Level = string.IsNullOrWhiteSpace(b.Level) ? null : b.Level.Trim(),
                    StartDate = ParseDateOrNull(b.StartDate),
                    EndDate = ParseDateOrNull(b.EndDate),
                });
            }
            return item;
        }

        [HttpPut("management/AditionalInfos/{id:guid}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAditionalInfoRequest req)
        {
            AditionalInfoItem? e = await _repo.GetAsync(id);
            if (e is null) return NotFound();

            e.AditionalInfo = req.AditionalInfo.Trim();
            e.Bullets.Clear();
            foreach (var b in req.Bullets ?? Array.Empty<AditionalInfoBulletDto>())
            {
                e.Bullets.Add(new AditionalInfoBullet
                {
                    Text = b.Text.Trim(),
                    Level = string.IsNullOrWhiteSpace(b.Level) ? null : b.Level.Trim(),
                    StartDate = ParseDateOrNull(b.StartDate),
                    EndDate = ParseDateOrNull(b.EndDate),
                });
            }
            e.UpdatedAt = DateTime.UtcNow;

            await _repo.UpdateAsync(e);
            await _repo.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("management/AditionalInfos/{id:guid}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            AditionalInfoItem? e = await _repo.GetAsync(id);
            if (e is null) return NotFound();
            await _repo.DeleteAsync(e);
            await _repo.SaveChangesAsync();
            return NoContent();
        }
        private static AditionalInfoResponse MapResponse(AditionalInfoItem e) => new(
            e.Id,
            e.AditionalInfo,
            e.Bullets.Select(b => new AditionalInfoBulletDto(
                b.Text,
                b.Level,
                b.StartDate?.ToString("yyyy-MM-dd"),
                b.EndDate?.ToString("yyyy-MM-dd")
            )).ToArray(),
            e.CreatedAt,
            e.UpdatedAt
        );

        private static DateOnly? ParseDateOrNull(string? iso)
        {
            if (string.IsNullOrWhiteSpace(iso)) return null;
            return DateOnly.TryParse(iso, out var d) ? d : null;
        }
    }
}
