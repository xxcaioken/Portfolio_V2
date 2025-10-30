using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio_V2.Application.Services;
using Portfolio_V2.Contracts;
using Portfolio_V2.Domain.Models;

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

			User? user = await _authService.ValidateUserAsync(request.Username, request.Password);
			if (user is null)
			{
				return Unauthorized();
			}
			
			List<Claim> extraClaims = new()
            {
				new Claim(ClaimTypes.Name, user.Username),
				new Claim(ClaimTypes.Role, user.Role.ToString()),
			};

			(string token, DateTime expiresAt) = _tokenService.CreateToken(user!.Username,  [..extraClaims]);
			return Ok(new AuthResponse(token, expiresAt));
		}

		[HttpGet("me")]
		[Authorize]
		public ActionResult<object> Me()
		{
			string name = User.Identity?.Name ?? "unknown";
			IEnumerable<object> claims = User.Claims.Select(c => new { c.Type, c.Value });
			return Ok(new { name, claims });
		}
	}
}
