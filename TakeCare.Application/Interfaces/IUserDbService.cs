using TakeCare.Database.Entity;

namespace TakeCare.Application.Interfaces
{
    public interface IUserDbService
    {
       void AddUserAsync(User user);
    }
}
