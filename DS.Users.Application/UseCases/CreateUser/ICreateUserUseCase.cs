using DS.Shared.Results;

namespace DS.Users.Application.UseCases.CreateUser
{
    public interface ICreateUserUseCase
    {       
        Task<Result<Guid>> CreateUserAsync(CreateUserRequest request, CancellationToken cancellationToken);
        public record CreateUserRequest(string FirstName, string LastName, string Email);
    }
}
