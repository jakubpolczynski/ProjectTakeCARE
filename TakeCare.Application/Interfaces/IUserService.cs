using TakeCare.Database.Entity;

namespace TakeCare.Application.Interfaces
{
    public interface IUserService
    {
        Task<string> CreateUserAsync(User user);

        Task<User> ReadUserAsync(int userId);

        Task<string> UpdateUserAsync(User user);

        Task<string> DeleteUserAsync(int userId);

        Task<bool> CheckIfUserExistAsync(int userId);
    }
}
