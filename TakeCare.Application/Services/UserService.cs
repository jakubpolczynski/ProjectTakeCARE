using Couchbase.Core.Exceptions;
using TakeCare.Database.Data;
using TakeCare.Database.Entity;
using TakeCare.Application.Interfaces;

namespace TakeCare.Application.Services
{
    public class UserService : IUserService
    {
        private readonly TakeCareDbContext _context;

        public UserService(TakeCareDbContext context)
        {
			_context = context ?? throw new InvalidArgumentException("Invalid context");
        }

        public async Task<string> CreateUserAsync(User user)
        {
			// Check if user is null
            if (user == null) throw new InvalidArgumentException("Invalid user provided");

			// Check if user already exists
            var userExists = await ReadUserAsync(user.Id);
			if (userExists == user)
            {
				throw new InvalidArgumentException("User already exists");
            }

			// Add user to database
            await _context.UserTable.AddAsync(user);
            
			// Save changes
            var userAdded = await _context.SaveChangesAsync();

			// Check if user was added
            if (userAdded == 1)
            {
	            return "User added";
            }
			throw new InvalidArgumentException("User not added");
        }

		public async Task<string> DeleteUserAsync(int userId)
		{
			// Check if user exists
			var userExists = await CheckIfUserExistAsync(userId);
			if (!userExists)
			{
				throw new InvalidArgumentException("User not found");
			}

			// Read user
			var user = await ReadUserAsync(userId);
			
			// Delete user
			_context.UserTable.Remove(user);

			// Save changes
			var userDeleted = await _context.SaveChangesAsync();
			
			// Check if user was deleted
			if (userDeleted == 1)
			{
				return "User deleted";
			}
			throw new InvalidArgumentException("User not deleted");
		}

		public async Task<User> ReadUserAsync(int userId)
		{
			// Check if user id is valid
			if (userId < 1)
			{
				throw new InvalidArgumentException("Invalid user id");
			}

			// Read user
			var user = await _context.UserTable.FindAsync(userId);
			
			// Return user
			return user ?? throw new InvalidArgumentException("User not found");

		}

		public async Task<string> UpdateUserAsync(User user)
		{
			// Check if user exists
			var userExists = await CheckIfUserExistAsync(user.Id);
			if (!userExists)
			{
				throw new InvalidArgumentException("User not found");
			}

			// Update user
			_context.UserTable.Update(user);

			// Save changes
			var userUpdated = await _context.SaveChangesAsync();

			// Check if user was updated
			if (userUpdated == 1)
			{
				return "User updated";
			}
			throw new InvalidArgumentException("User not updated");
		}

		public async Task<bool> CheckIfUserExistAsync(int userId)
		{
			// Check if user id is valid
			if (userId < 1)
			{
				throw new InvalidArgumentException("Invalid user id");
			}

			// Check if user exists
			var user = await _context.UserTable.FindAsync(userId);

			// Return true if user exists
			return user != null;
		}
	}
}
