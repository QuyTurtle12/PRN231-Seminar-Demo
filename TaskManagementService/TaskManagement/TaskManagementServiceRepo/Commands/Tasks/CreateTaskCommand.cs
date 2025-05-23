using MediatR;
using TaskManagementServiceDAO.Interfaces;
using TaskManagementServiceRepo.Interfaces;

namespace TaskManagementServiceRepo.Commands.Tasks;

public record CreateTaskCommand(TaskManagementServiceBO.Task Task) : ICommand<int>;

public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, int>
{
    private readonly ITaskDAO _taskDAO;

    public CreateTaskCommandHandler(ITaskDAO taskDAO)
    {
        _taskDAO = taskDAO;
    }

    public async Task<int> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        return await _taskDAO.AddTask(request.Task, cancellationToken);
    }
}
