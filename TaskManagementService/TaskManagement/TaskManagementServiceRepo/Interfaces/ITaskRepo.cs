using TaskManagementServiceDAO.DTOs;

namespace TaskManagementServiceRepo.Interfaces
{
    public interface ITaskRepo
    {
        Task<List<TaskDTO>> GetAllTasksWithUserNameASync(CancellationToken cancellationToken = default);
        Task<int> AddTask(TaskManagementServiceBO.Task task, CancellationToken cancellationToken = default);
        Task UpdateTaskAsync(TaskManagementServiceBO.Task task, CancellationToken cancellationToken = default);
        Task DeleteTaskAsync(int taskId, CancellationToken cancellationToken = default);
    }
}
