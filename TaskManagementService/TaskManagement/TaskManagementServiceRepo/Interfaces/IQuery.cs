using MediatR;

namespace TaskManagementServiceRepo.Interfaces
{
    public interface IQuery<out TResult> : IRequest<TResult>
    {
    }
}
