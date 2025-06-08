using TaskManagementServiceDAO.DTOs;

namespace TaskManagementServiceRepo.Interfaces
{
    public interface ITaskRepo
    {
        Task<List<TaskDTO>> GetAllTasksWithUserNameASync(CancellationToken cancellationToken = default);
    }
}
