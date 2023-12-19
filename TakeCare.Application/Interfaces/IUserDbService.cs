using TakeCare.Database.Entity;

namespace TakeCare.Application.Interfaces
{
    public interface IUserDbService
    {
       Task AddUserAsync(User user);
    }
}
