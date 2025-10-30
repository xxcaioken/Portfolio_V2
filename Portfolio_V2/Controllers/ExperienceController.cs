using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio_V2.Contracts;
using Portfolio_V2.Domain.Models;
using Portfolio_V2.Infrastructure.Repositories;
using System.Globalization;

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
            List<ExperienceItem> list = await _repo.ListAsync();
            return Ok(list.Select(MapResponse));
        }

        [HttpGet("experiences/{id:guid}")]
        [AllowAnonymous]
        public async Task<ActionResult<ExperienceResponse>> Get(Guid id)
        {
            ExperienceItem? e = await _repo.GetAsync(id);
            if (e is null) return NotFound();
            return Ok(MapResponse(e));
        }

        [HttpPost("management/experiences")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<ExperienceResponse>> Create([FromBody] CreateExperienceRequest req)
        {
            if (!TryParseAndValidateDates(req.StartDate, req.EndDate, out var start, out var end, out var problem))
                return ValidationProblem(problem);

            ExperienceItem e = new ExperienceItem
            {
                Company = req.Company.Trim(),
                Role = req.Role.Trim(),
                StartDate = start,
                EndDate = end,
                Bullets = req.Bullets ?? Array.Empty<string>(),
            };
            await _repo.AddAsync(e);
            await _repo.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = e.Id }, MapResponse(e));
        }

        [HttpPut("management/experiences/{id:guid}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateExperienceRequest req)
        {
            ExperienceItem? e = await _repo.GetAsync(id);
            if (e is null) return NotFound();
            if (!TryParseAndValidateDates(req.StartDate, req.EndDate, out var start, out var end, out var problem))
                return ValidationProblem(problem);

            e.Company = req.Company.Trim();
            e.Role = req.Role.Trim();
            e.StartDate = start;
            e.EndDate = end;
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
            ExperienceItem? e = await _repo.GetAsync(id);
            if (e is null) return NotFound();
            await _repo.DeleteAsync(e);
            await _repo.SaveChangesAsync();
            return NoContent();
        }
        private static ExperienceResponse MapResponse(ExperienceItem e) => new ExperienceResponse(
            e.Id,
            e.Company,
            e.Role,
            e.StartDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
            e.EndDate?.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
            e.Bullets,
            e.CreatedAt,
            e.UpdatedAt
        );

        private static bool TryParseAndValidateDates(string startStr, string? endStr, out DateOnly start, out DateOnly? end, out ValidationProblemDetails problem)
        {
            problem = new ValidationProblemDetails();
            if (!DateOnly.TryParseExact(startStr, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out start))
            {
                problem.Errors[nameof(startStr)] = new[] { "Formato de data inválido. Use yyyy-MM-dd." };
                end = null;
                return false;
            }

            if (!string.IsNullOrWhiteSpace(endStr))
            {
                if (!DateOnly.TryParseExact(endStr!, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var endParsed))
                {
                    problem.Errors[nameof(endStr)] = new[] { "Formato de data inválido. Use yyyy-MM-dd." };
                    end = null;
                    return false;
                }
                end = endParsed;

                if (end.Value < start)
                {
                    problem.Errors[nameof(endStr)] = new[] { "EndDate não pode ser anterior a StartDate." };
                    return false;
                }
            }
            else
            {
                end = null;
            }
            return true;
        }
    }
}


