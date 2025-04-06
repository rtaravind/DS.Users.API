using DS.Shared.Results;
using DS.Users.Application.Commands.UpdateUser;
using DS.Users.Application.UseCases.CreateUser;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DS.Users.Application.UseCases.UpdateUser
{
    public class UpdateUserUseCase(
        ISender sender,
        ILogger<CreateUserUseCase> logger) : IUpdateUserUseCase
    {
        public async Task<Result<Unit>> UpdateUserAsync(IUpdateUserUseCase.UpdateUserRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateUserCommand
            {
                Id = request.Id,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email
            };

            var result = await sender.Send(command, cancellationToken);

            if (result.IsSuccess)
            {
                return Result<Unit>.Success(Unit.Value);
            }

            logger.LogInformation("User updated successfully with ID [{UserId}]", request.Id);

            return Result<Unit>.Failure(result.Error);
        }
    }
}
