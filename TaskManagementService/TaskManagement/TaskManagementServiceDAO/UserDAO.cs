using TaskManagementServiceBO;
using TaskManagementServiceDAO.Interfaces;

namespace TaskManagementServiceDAO
{
    public class UserDAO : IUserDAO
    {
        private readonly TaskManagementContext _context;

        public UserDAO(TaskManagementContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserById(int userId, CancellationToken cancellationToken = default)
        {
            return await _context.Users.FindAsync(userId, cancellationToken);
        }
    }
}
