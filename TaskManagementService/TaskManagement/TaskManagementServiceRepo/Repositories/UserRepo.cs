using Microsoft.Extensions.DependencyInjection;
using Polly.Bulkhead;
using TaskManagementServiceBO;
using TaskManagementServiceDAO.DTOs;
using TaskManagementServiceDAO.Interfaces;
using TaskManagementServiceRepo.Interfaces;

namespace TaskManagementServiceRepo.Repositories
{
    public class UserRepo : IUserRepo
    {
        private readonly IUserDAO _userDAO;
        private readonly AsyncBulkheadPolicy _bulkheadPolicy;

        public UserRepo(IUserDAO userDAO, [FromKeyedServices("UsersBulkhead")] AsyncBulkheadPolicy bulkheadPolicy)
        {
            _userDAO = userDAO;
            _bulkheadPolicy = bulkheadPolicy;
        }

        public async Task<string> GetUserNameAsync(int userId, CancellationToken cancellationToken = default)
        {
            return await _userDAO.GetUserById(userId, cancellationToken) switch
            {
                null => $"User {userId} not found",
                User user => user.Name ?? $"User {userId} has no name"
            };
        }

        public async Task<IList<UserDTO>> GetAllUsers(CancellationToken cancellationToken = default)
        {
            return await _bulkheadPolicy.ExecuteAsync(async ct => {
                IList<User> users = await _userDAO.GetAllUsers(cancellationToken);

                IList<UserDTO> result = users.Select(user => new UserDTO
                {
                    Name = user.Name ?? $"User {user.Id} has no name",
                    Email = user.Email ?? $"User {user.Id} has no email"
                }).ToList();

                return result;
            }, cancellationToken);
        }
    }
}
