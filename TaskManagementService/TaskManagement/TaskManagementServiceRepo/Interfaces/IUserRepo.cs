namespace TaskManagementServiceRepo.Interfaces
{
    public interface IUserRepo
    {
        Task<string> GetUserNameAsync(int userId, CancellationToken cancellationToken = default);
    }
}
