using TaskManagementServiceBO;
using TaskManagementServiceDAO.Interfaces;
using TaskManagementServiceRepo.Interfaces;

namespace TaskManagementServiceRepo.Repositories
{
    public class UserRepo : IUserRepo
    {
        private readonly IUserDAO _userDAO;

        public UserRepo(IUserDAO userDAO)
        {
            _userDAO = userDAO;
        }

        private readonly Random _random = new Random();

        public async Task<string> GetUserNameAsync(int userId, CancellationToken cancellationToken = default)
        {
            return await _userDAO.GetUserById(userId, cancellationToken) switch
            {
                null => $"User {userId} not found",
                User user => user.Name ?? $"User {userId} has no name"
            };
        }
    }
}
