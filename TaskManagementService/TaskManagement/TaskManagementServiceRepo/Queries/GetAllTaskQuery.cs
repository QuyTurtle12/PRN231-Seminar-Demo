using MediatR;
using TaskManagementServiceDAO.Interfaces;
using TaskManagementServiceRepo.Interfaces;

namespace TaskManagementServiceRepo.Queries
{
    public class GetAllTaskQuery : IQuery<List<TaskManagementServiceBO.Task>>
    {
        public record GetAllTaskQueryRequest() : IQuery<List<TaskManagementServiceBO.Task>>;

        public class GetAllTaskQueryHandler : IRequestHandler<GetAllTaskQueryRequest, List<TaskManagementServiceBO.Task>>
        {
            private readonly ITaskDAO _taskDAO;
            public GetAllTaskQueryHandler(ITaskDAO taskDAO)
            {
                _taskDAO = taskDAO;
            }
            public async Task<List<TaskManagementServiceBO.Task>> Handle(GetAllTaskQueryRequest request, CancellationToken cancellationToken)
            {
                return await _taskDAO.GetAllTasks(cancellationToken);
            }
        }

    }
}
