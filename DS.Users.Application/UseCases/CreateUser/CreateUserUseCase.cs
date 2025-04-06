using DS.Shared.Results;
using DS.Users.Application.Commands.CreateUser;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DS.Users.Application.UseCases.CreateUser
{
    public class CreateUserUseCase(
        ISender sender,
        ILogger<CreateUserUseCase> logger) : ICreateUserUseCase
    {
        public async Task<Result<Guid>> CreateUserAsync(ICreateUserUseCase.CreateUserRequest request, CancellationToken cancellationToken)
        {
            var command = new CreateUserCommand
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email
            };

            var result = await sender.Send(command, cancellationToken);

            if (!result.IsSuccess)
            {
                logger.LogWarning("User creation failed: {Error}", result.Error);
                return Result<Guid>.Failure(result.Error);
            }

            logger.LogInformation("User created successfully with ID [{UserId}]", result.Value);

            return Result<Guid>.Success(result.Value);
        }
    }
}
