using TaskManagementServiceBO;

namespace TaskManagementServiceDAO.Interfaces
{
    public interface IUserDAO
    {
        Task<User?> GetUserById(int userId, CancellationToken cancellationToken = default);
        Task<List<User>> GetAllUsers(CancellationToken cancellationToken = default);
    }
}
