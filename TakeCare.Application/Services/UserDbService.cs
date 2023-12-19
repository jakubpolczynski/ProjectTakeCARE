using Couchbase.Core.Exceptions;
using TakeCare.Database.Data;
using TakeCare.Database.Entity;
using TakeCare.Application.Interfaces;

namespace TakeCare.Application.Services
{
    public class UserDbService : IUserDbService
    {
        private readonly TakeCareDBContext _context;

        public UserDbService(TakeCareDBContext context)
        {
			_context = context ?? throw new InvalidArgumentException("context");
        }

        public async Task AddUserAsync(User user)
        {
            if (user == null) throw new InvalidArgumentException("user");
            await _context.UserTable.AddAsync(user);
            await _context.SaveChangesAsync();
        }
    }
}
