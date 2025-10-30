using System.Security.Cryptography;
using System.Text;
using Portfolio_V2.Domain.Models;
using Portfolio_V2.Infrastructure.Repositories;

namespace Portfolio_V2.Application.Services
{
	public class AuthService(IUserRepository userRepository) : IAuthService
	{
		private const int SaltSize = 16;
		private const int KeySize = 32;
		private const int Iterations = 100_000;
		private static readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA256;

		private readonly IUserRepository _userRepository = userRepository;

        public async Task<User?> ValidateUserAsync(string username, string password)
		{
			var user = await _userRepository.GetByUsernameAsync(username);
			if (user == null)
			{
				return null;
			}

			var computedHash = HashPassword(password, Convert.FromBase64String(user.PasswordSalt));
			return TimingSafeEquals(Convert.FromBase64String(user.PasswordHash), computedHash) ? user : null;
		}

		public static (string Hash, string Salt) CreatePasswordHash(string password)
		{
			var salt = RandomNumberGenerator.GetBytes(SaltSize);
			var hash = HashPassword(password, salt);
			return (Convert.ToBase64String(hash), Convert.ToBase64String(salt));
		}

		private static byte[] HashPassword(string password, byte[] salt)
		{
			return Rfc2898DeriveBytes.Pbkdf2(
				password,
				salt,
				Iterations,
				Algorithm,
				KeySize
			);
		}

		private static bool TimingSafeEquals(ReadOnlySpan<byte> a, ReadOnlySpan<byte> b)
		{
			return CryptographicOperations.FixedTimeEquals(a, b);
		}
	}
}
