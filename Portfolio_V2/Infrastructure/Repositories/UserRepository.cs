using Microsoft.EntityFrameworkCore;
using Portfolio_V2.Domain.Models;

namespace Portfolio_V2.Infrastructure.Repositories
{
	public class UserRepository(AppDbContext dbContext) : IUserRepository
	{
		private readonly AppDbContext _dbContext = dbContext;

        public async Task<User?> GetByUsernameAsync(string username)
		{
			return await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Username == username);
		}

		public async Task AddAsync(User user)
		{
			await _dbContext.Users.AddAsync(user);
		}

		public async Task SaveChangesAsync()
		{
			await _dbContext.SaveChangesAsync();
		}
	}
}
