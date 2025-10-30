using System.Threading.Tasks;
using Portfolio_V2.Domain.Models;

namespace Portfolio_V2.Infrastructure.Repositories
{
	public interface IUserRepository
	{
		Task<User?> GetByUsernameAsync(string username);
		Task AddAsync(User user);
		Task SaveChangesAsync();
	}
}
