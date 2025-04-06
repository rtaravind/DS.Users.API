using DS.Shared.Results;
using DS.Users.Application.DTOs;

namespace DS.Users.Application.UseCases.GetUser
{
    public interface IGetUserByIdUseCase
    {
        Task<Result<UserDto>> GetUserByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}
