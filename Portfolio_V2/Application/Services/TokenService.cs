using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Portfolio_V2.Application.Services
{
	public class TokenService(IConfiguration configuration) : ITokenService
	{
		private readonly IConfiguration _configuration = configuration;

        public (string Token, DateTime ExpiresAt) CreateToken(string username, Claim[]? extraClaims = null)
		{
			var jwtSection = _configuration.GetSection("Authentication:Jwt");
			var issuer = jwtSection.GetValue<string>("Issuer");
			var audience = jwtSection.GetValue<string>("Audience");
			var secret = jwtSection.GetValue<string>("Secret") ?? string.Empty;
			var lifetimeMinutes = jwtSection.GetValue<int>("TokenLifetimeMinutes");

			var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
			var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, username),
				new Claim(ClaimTypes.NameIdentifier, username)
			};
			if (extraClaims != null)
			{
				claims.AddRange(extraClaims);
			}

			var expiresAt = DateTime.UtcNow.AddMinutes(lifetimeMinutes > 0 ? lifetimeMinutes : 60);
			var token = new JwtSecurityToken(
				issuer: issuer,
				audience: audience,
				claims: claims,
				expires: expiresAt,
				signingCredentials: creds
			);

			var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
			return (tokenString, expiresAt);
		}
	}
}
