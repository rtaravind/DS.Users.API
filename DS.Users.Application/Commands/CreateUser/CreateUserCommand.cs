using DS.Shared.Results;
using MediatR;

namespace DS.Users.Application.Commands.CreateUser
{
    public class CreateUserCommand : IRequest<Result<Guid>>
    {
        public string FirstName { get; init; } = null!;
        public string LastName { get; init; } = null!;
        public string Email { get; init; } = null!;
    }
}
