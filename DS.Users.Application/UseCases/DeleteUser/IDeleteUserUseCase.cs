using DS.Shared.Results;

namespace DS.Users.Application.UseCases.DeleteUser
{
    public interface IDeleteUserUseCase
    {
        Task<Result<bool>> DeleteUserAsync(Guid id, CancellationToken cancellationToken);
    }
}
