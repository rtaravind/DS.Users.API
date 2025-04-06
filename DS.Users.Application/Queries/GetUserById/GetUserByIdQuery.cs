using DS.Shared.Results;
using DS.Users.Application.DTOs;
using MediatR;

namespace DS.Users.Application.Queries.GetUserById
{
    public class GetUserByIdQuery : IRequest<Result<UserDto>>
    {
        public Guid Id { get; set; }
    }
}
