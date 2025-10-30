using System.Threading.Tasks;
using Portfolio_V2.Domain.Models;

namespace Portfolio_V2.Application.Services
{
	public interface IAuthService
	{
		Task<User?> ValidateUserAsync(string username, string password);
	}
}
