using Polly.Bulkhead;
using TaskManagementServiceDAO.DTOs;
using TaskManagementServiceDAO.Interfaces;
using TaskManagementServiceRepo.Interfaces;

namespace TaskManagementServiceRepo.Repositories
{
    public class TaskRepo : ITaskRepo
    {
        private readonly ITaskDAO _taskDAO;
        private readonly IUserRepo _userService;

        public TaskRepo(ITaskDAO taskDAO, IUserRepo userService)
        {
            _taskDAO = taskDAO;
            _userService = userService;
        }

        public Task<int> AddTask(TaskManagementServiceBO.Task task, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task DeleteTaskAsync(int taskId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<List<TaskDTO>> GetAllTasksWithUserNameASync(CancellationToken cancellationToken = default)
        {

            var tasks = await _taskDAO.GetAllTasks(cancellationToken);

            var result = new List<TaskDTO>();

            foreach (var task in tasks)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var assignments = task.TaskAssignments;
                var userNames = new List<string>();

                foreach (var assignment in assignments)
                {
                    string userName = await _userService.GetUserNameAsync(assignment.UserId, cancellationToken);
                    userNames.Add(userName);
                }

                result.Add(new TaskDTO
                {
                    Id = task.Id,
                    Title = task.Title,
                    Description = task.Description,
                    Status = task.Status.Name,
                    Category = task.Category.Name,
                    AssignedUserNames = userNames
                });
            }
            return result;
        }

        public System.Threading.Tasks.Task UpdateTaskAsync(TaskManagementServiceBO.Task task, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
