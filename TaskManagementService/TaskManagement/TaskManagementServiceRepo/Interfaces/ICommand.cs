using MediatR;

namespace TaskManagementServiceRepo.Interfaces;
// For commands that return a value
public interface ICommand<out TResult> : IRequest<TResult>
{
}

// For commands that don't return a value
public interface ICommand : IRequest<Unit>
{
}
