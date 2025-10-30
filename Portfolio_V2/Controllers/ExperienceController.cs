using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio_V2.Contracts;
using Portfolio_V2.Domain.Models;
using Portfolio_V2.Infrastructure.Repositories;

namespace Portfolio_V2.Controllers
{
    [ApiController]
    [Route("")]
    public class ExperienceController(IExperienceRepository repo) : ControllerBase
    {
        private readonly IExperienceRepository _repo = repo;

        [HttpGet("experiences")]
        [AllowAnonymous]
        public async Task<ActionResult<List<ExperienceResponse>>> List()
        {
            var list = await _repo.ListAsync();
            return Ok(list.Select(e => new ExperienceResponse(e.Id, e.Company, e.Role, e.Period, e.Bullets, e.CreatedAt, e.UpdatedAt)));
        }

        [HttpGet("experiences/{id:guid}")]
        [AllowAnonymous]
        public async Task<ActionResult<ExperienceResponse>> Get(Guid id)
        {
            var e = await _repo.GetAsync(id);
            if (e is null) return NotFound();
            return Ok(new ExperienceResponse(e.Id, e.Company, e.Role, e.Period, e.Bullets, e.CreatedAt, e.UpdatedAt));
        }

        [HttpPost("management/experiences")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<ExperienceResponse>> Create([FromBody] CreateExperienceRequest req)
        {
            var e = new ExperienceItem
            {
                Company = req.Company.Trim(),
                Role = req.Role.Trim(),
                Period = req.Period.Trim(),
                Bullets = req.Bullets ?? Array.Empty<string>(),
            };
            await _repo.AddAsync(e);
            await _repo.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = e.Id },
                new ExperienceResponse(e.Id, e.Company, e.Role, e.Period, e.Bullets, e.CreatedAt, e.UpdatedAt));
        }

        [HttpPut("management/experiences/{id:guid}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateExperienceRequest req)
        {
            var e = await _repo.GetAsync(id);
            if (e is null) return NotFound();
            e.Company = req.Company.Trim();
            e.Role = req.Role.Trim();
            e.Period = req.Period.Trim();
            e.Bullets = req.Bullets ?? Array.Empty<string>();
            e.UpdatedAt = DateTime.UtcNow;
            await _repo.UpdateAsync(e);
            await _repo.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("management/experiences/{id:guid}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var e = await _repo.GetAsync(id);
            if (e is null) return NotFound();
            await _repo.DeleteAsync(e);
            await _repo.SaveChangesAsync();
            return NoContent();
        }
    }
}


