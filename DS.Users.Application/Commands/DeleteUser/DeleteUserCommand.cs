using DS.Shared.Results;
using MediatR;

namespace DS.Users.Application.Commands.DeleteUser
{
    public class DeleteUserCommand : IRequest<Result<Unit>>
    {
        public Guid Id { get; set; }
    }
}
