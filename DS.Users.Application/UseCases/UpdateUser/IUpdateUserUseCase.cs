using DS.Shared.Results;
using MediatR;

namespace DS.Users.Application.UseCases.UpdateUser
{
    public interface IUpdateUserUseCase
    {
        Task<Result<Unit>> UpdateUserAsync(UpdateUserRequest request, CancellationToken cancellationToken);

        public record UpdateUserRequest(Guid Id, string FirstName, string LastName, string Email);
    }
}
