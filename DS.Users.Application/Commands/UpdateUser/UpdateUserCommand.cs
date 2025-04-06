using DS.Shared.Results;
using MediatR;

namespace DS.Users.Application.Commands.UpdateUser
{
    public class UpdateUserCommand : IRequest<Result<Unit>>
    {
        public Guid Id { get; set; }
        public string FirstName { get; init; } = null!;
        public string LastName { get; init; } = null!;
        public string Email { get; init; } = null!;
    }
}
