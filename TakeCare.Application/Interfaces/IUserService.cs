using TakeCare.Database.Entity;

namespace TakeCare.Application.Interfaces
{
	public interface IUserService
	{
		Task<bool> CheckIfUserExistsAsync(string email);
		Task<User> GetUserByEmailAsync(string email);
	}
}