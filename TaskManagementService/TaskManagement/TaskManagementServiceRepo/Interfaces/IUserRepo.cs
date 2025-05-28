using TaskManagementServiceDAO.DTOs;

namespace TaskManagementServiceRepo.Interfaces
{
    public interface IUserRepo
    {
        Task<string> GetUserNameAsync(int userId, CancellationToken cancellationToken = default);
        Task<IList<UserDTO>> GetAllUsers(CancellationToken cancellationToken = default);
    }
}
