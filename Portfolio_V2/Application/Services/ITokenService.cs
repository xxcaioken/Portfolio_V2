using System;
using System.Security.Claims;

namespace Portfolio_V2.Application.Services
{
	public interface ITokenService
	{
		(string Token, DateTime ExpiresAt) CreateToken(string username, Claim[]? extraClaims = null);
	}
}
