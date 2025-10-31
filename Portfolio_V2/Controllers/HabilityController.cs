using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio_V2.Contracts;
using Portfolio_V2.Domain.Models;
using Portfolio_V2.Infrastructure.Repositories;

namespace Portfolio_V2.Controllers
{
    [ApiController]
    [Route("")]
    public class HabilityController(IHabilityRepository repo) : ControllerBase
    {
        private readonly IHabilityRepository _repo = repo;

        [HttpGet("Habilitys")]
        [AllowAnonymous]
        public async Task<ActionResult<List<HabilityResponse>>> List()
        {
            List<HabilityItem> list = await _repo.ListAsync();
            return Ok(list.Select(MapResponse));
        }

        [HttpGet("Habilitys/{id:guid}")]
        [AllowAnonymous]
        public async Task<ActionResult<HabilityResponse>> Get(Guid id)
        {
            HabilityItem? e = await _repo.GetAsync(id);
            if (e is null) return NotFound();
            return Ok(MapResponse(e));
        }

        [HttpPost("management/Habilitys")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<HabilityResponse>> Create([FromBody] CreateHabilityRequest req)
        {   
            HabilityItem e = GetE(req);
            await _repo.AddAsync(e);
            await _repo.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = e.Id }, MapResponse(e));
        }

        private static HabilityItem GetE(CreateHabilityRequest req)
        {
            return new()
            {
                Hability= req.Hability.Trim(),
                Badge= req.Badge.Trim(),
                Bullets = req.Bullets ?? [],
            };
        }

        [HttpPut("management/Habilitys/{id:guid}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateHabilityRequest req)
        {
            HabilityItem? e = await _repo.GetAsync(id);
            if (e is null) return NotFound();

            e.Hability = req.Hability.Trim();
            e.Bullets = req.Bullets ?? [];
            e.Badge = req.Badge.Trim();
            e.UpdatedAt = DateTime.UtcNow;
            
            await _repo.UpdateAsync(e);
            await _repo.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("management/Habilitys/{id:guid}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            HabilityItem? e = await _repo.GetAsync(id);
            if (e is null) return NotFound();
            await _repo.DeleteAsync(e);
            await _repo.SaveChangesAsync();
            return NoContent();
        }
        private static HabilityResponse MapResponse(HabilityItem e) => new(
            e.Id,
            e.Hability,
            e.Bullets,
            e.Badge,
            e.CreatedAt,
            e.UpdatedAt
        );
    }
}
