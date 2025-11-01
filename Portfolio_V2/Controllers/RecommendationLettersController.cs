using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio_V2.BLL;
using Portfolio_V2.Contracts;
using Portfolio_V2.Domain.Models;
using Portfolio_V2.Infrastructure.Repositories;

namespace Portfolio_V2.Controllers
{
    [ApiController]
    [Route("")]
    public class RecommendationLettersController(IRecommendationLetterRepository repo, IRecommendationLetterBll bll) : ControllerBase
    {
        private readonly IRecommendationLetterRepository _repo = repo;
        private readonly IRecommendationLetterBll _bll = bll;

        [HttpGet("RecommendationLetters")]
        [AllowAnonymous]
        public async Task<ActionResult<List<RecommendationLetterResponse>>> List()
        {
            var list = await _bll.ListAsync();
            return Ok(list);
        }

        [HttpGet("RecommendationLetters/{id:guid}")]
        [AllowAnonymous]
        public async Task<ActionResult<RecommendationLetterResponse>> Get(Guid id)
        {
            var dto = await _bll.GetAsync(id);
            if (dto is null) return NotFound();
            return Ok(dto);
        }

        [HttpPost("management/RecommendationLetters")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<RecommendationLetterResponse>> Create([FromBody] CreateRecommendationLetterRequest req)
        {
            var e = new RecommendationLetter
            {
                ImageUrlPt = string.IsNullOrWhiteSpace(req.ImageUrlPt) ? null : req.ImageUrlPt.Trim(),
                ImageUrlEn = string.IsNullOrWhiteSpace(req.ImageUrlEn) ? null : req.ImageUrlEn.Trim(),
            };
            await _repo.AddAsync(e);
            await _repo.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = e.Id }, new RecommendationLetterResponse(e.Id, e.ImageUrlPt, e.ImageUrlEn, e.CreatedAt, e.UpdatedAt));
        }

        [HttpPut("management/RecommendationLetters/{id:guid}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRecommendationLetterRequest req)
        {
            var e = await _repo.GetAsync(id);
            if (e is null) return NotFound();
            e.ImageUrlPt = string.IsNullOrWhiteSpace(req.ImageUrlPt) ? null : req.ImageUrlPt.Trim();
            e.ImageUrlEn = string.IsNullOrWhiteSpace(req.ImageUrlEn) ? null : req.ImageUrlEn.Trim();
            e.UpdatedAt = DateTime.UtcNow;
            await _repo.UpdateAsync(e);
            await _repo.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("management/RecommendationLetters/{id:guid}")]
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


