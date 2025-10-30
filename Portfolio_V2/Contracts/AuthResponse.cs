using System;

namespace Portfolio_V2.Contracts
{
	public record AuthResponse(string Token, DateTime ExpiresAt);
}
