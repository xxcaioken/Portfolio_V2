using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio_V2.Application.Services;
using Portfolio_V2.Contracts;

namespace Portfolio_V2.Controllers
{
	[ApiController]
	[Route("auth")]
	public class AuthController(IAuthService authService, ITokenService tokenService) : ControllerBase
	{
		private readonly IAuthService _authService = authService;
		private readonly ITokenService _tokenService = tokenService;

        [HttpPost("login")]
		[AllowAnonymous]
		public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
		{
			if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
			{
				return BadRequest(new { error = "Credenciais inválidas" });
			}

			var user = await _authService.ValidateUserAsync(request.Username, request.Password);
			if (user == null)
			{
				return Unauthorized();
			}

			var (token, expiresAt) = _tokenService.CreateToken(user.Username);
			return Ok(new AuthResponse(token, expiresAt));
		}

		[HttpGet("me")]
		[Authorize]
		public ActionResult<object> Me()
		{
			var name = User.Identity?.Name ?? "unknown";
			var claims = User.Claims.Select(c => new { c.Type, c.Value });
			return Ok(new { name, claims });
		}
	}
}
