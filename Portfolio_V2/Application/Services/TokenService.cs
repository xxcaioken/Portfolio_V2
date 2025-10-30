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
			IConfigurationSection jwtSection = _configuration.GetSection("Authentication:Jwt");
			string issuer = jwtSection.GetValue<string>("Issuer") ?? string.Empty;
			string audience = jwtSection.GetValue<string>("Audience") ?? string.Empty;
			string secret = jwtSection.GetValue<string>("Secret") ?? string.Empty;
			int lifetimeMinutes = jwtSection.GetValue<int>("TokenLifetimeMinutes");

			SymmetricSecurityKey signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
			SigningCredentials creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

			List<Claim> claims =
            [
                new Claim(ClaimTypes.Name, username),
				new Claim(ClaimTypes.NameIdentifier, username)
			];
			if (extraClaims != null)
			{
				claims.AddRange(extraClaims);
			}

			DateTime expiresAt = DateTime.UtcNow.AddMinutes(lifetimeMinutes > 0 ? lifetimeMinutes : 60);
			JwtSecurityToken token = new(
				issuer: issuer,
				audience: audience,
				claims: claims,
				expires: expiresAt,
				signingCredentials: creds
			);

			string tokenString = new JwtSecurityTokenHandler().WriteToken(token);
			return (tokenString, expiresAt);
		}
	}
}
