using Microsoft.EntityFrameworkCore;
using TakeCare.Application.Interfaces;
using TakeCare.Database.Data;
using TakeCare.Database.Entity;


namespace TakeCare.Application.Services
{
	public class UserService : IUserService
	{
		private readonly TakeCareDbContext _context;
		private readonly DbSet<User> _dbSet;

		public UserService(TakeCareDbContext context)
		{
			_context = context;
			_dbSet = _context.Set<User>();
		}

		public async Task<bool> CheckIfUserExistsAsync(string email)
		{
			User userExists;
			if(email != null)
			{
				userExists = await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
			}
			else
			{
				throw new ArgumentException("Invalid email provided");
			}

			return userExists != null;
		}

		public async Task<User> GetUserByEmailAsync(string email)
		{
			User user;
			if (email != null)
			{
				user = await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
			}
			else
			{
				throw new ArgumentException("Invalid data provided");
			}

			return user;
		}
	}
}
