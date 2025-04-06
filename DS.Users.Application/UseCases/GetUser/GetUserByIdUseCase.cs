using DS.Shared.Results;
using DS.Users.Application.DTOs;
using DS.Users.Application.Queries.GetUserById;
using MediatR;

namespace DS.Users.Application.UseCases.GetUser
{
    public class GetUserByIdUseCase(ISender sender) : IGetUserByIdUseCase
    {
        public async Task<Result<UserDto>> GetUserByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var query = new GetUserByIdQuery { Id = id };
            var result = await sender.Send(query, cancellationToken);

            return result is not null
                ? Result<UserDto>.Success(result.Value)
                : Result<UserDto>.Failure(result.Error);
        }
    }
}
