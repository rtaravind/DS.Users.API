using DS.Shared.Results;
using DS.Users.Application.Commands.DeleteUser;
using MediatR;

namespace DS.Users.Application.UseCases.DeleteUser
{
    public class DeleteUserUseCase(ISender sender) : IDeleteUserUseCase
    {
        public async Task<Result<bool>> DeleteUserAsync(Guid id, CancellationToken cancellationToken)
        {
            var command = new DeleteUserCommand { Id = id};
            var result = await sender.Send(command, cancellationToken);

            if(result.IsSuccess)
            {
                Result<bool>.Success(true);
            }

            return Result<bool>.Failure(result.Error);
        }
    }
}
