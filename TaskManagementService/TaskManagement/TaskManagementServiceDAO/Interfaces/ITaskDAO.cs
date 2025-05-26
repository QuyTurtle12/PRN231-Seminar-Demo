namespace TaskManagementServiceDAO.Interfaces
{
    public interface ITaskDAO
    {
        Task<List<TaskManagementServiceBO.Task>> GetAllTasks(CancellationToken cancellationToken = default);
        Task<int> AddTask(TaskManagementServiceBO.Task task, CancellationToken cancellationToken = default);
        System.Threading.Tasks.Task UpdateTaskAsync(TaskManagementServiceBO.Task task, CancellationToken cancellationToken = default);
        System.Threading.Tasks.Task DeleteTaskAsync(int taskId, CancellationToken cancellationToken = default);
    }
}
