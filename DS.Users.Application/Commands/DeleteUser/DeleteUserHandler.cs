using DS.Domain.Entities;
using DS.Shared.Logging;
using DS.Shared.Results;
using DS.Users.Application.Interfaces;
using MediatR;

namespace DS.Users.Application.Commands.DeleteUser
{
    public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, Result<Unit>>    
    {
        private readonly IBaseRepository<User> _repository;
        private readonly ILoggerManager _logger;

        public DeleteUserHandler(IBaseRepository<User> repository, ILoggerManager logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<Result<Unit>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _repository.GetByIdAsync(request.Id, cancellationToken);
                if (user is null)
                {
                    var error = $"User with ID {request.Id} not found.";
                    _logger.LogWarn(error);

                    return Result<Unit>.Failure(new Error("DeleteUser.Failed", error));
                }

                await _repository.DeleteAsync(user);
                await _repository.SaveChangesAsync(cancellationToken);

                _logger.LogInfo($"User with ID {request.Id} deleted.");

                return Result<Unit>.Success(Unit.Value);
            }

            catch (Exception ex)
            {
                _logger.LogError($"Error creating user: {ex.Message}");
                return Result<Unit>.Failure(new Error("DeleteUser.Failed", "An unexpected error occurred."));
            }            
        }
    }
}
