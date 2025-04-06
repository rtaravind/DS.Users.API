using DS.Domain.Entities;
using DS.Shared.Logging;
using DS.Shared.Results;
using DS.Users.Application.Interfaces;
using MediatR;

namespace DS.Users.Application.Commands.UpdateUser
{
    public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, Result<Unit>>
    {
        private readonly IBaseRepository<User> _repository;
        private readonly ILoggerManager _logger;
        public UpdateUserHandler(IBaseRepository<User> repository, ILoggerManager logger)
        {
            _repository = repository;
            _logger = logger;
        } 

        public async Task<Result<Unit>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _repository.GetByIdAsync(request.Id, cancellationToken);
                if (user is null)
                {
                    var error = $"User with ID {request.Id} not found.";
                    _logger.LogWarn(error);
                    return Result<Unit>.Failure(new Error("UpdateUser.Failed", error));
                }

                user.FirstName = request.FirstName;
                user.LastName = request.LastName;
                user.Email = request.Email;
                user.LastModified = DateTime.UtcNow;

                await _repository.UpdateAsync(user);
                await _repository.SaveChangesAsync(cancellationToken);

                _logger.LogInfo($"User with ID {request.Id} updated.");

                return Result<Unit>.Success(Unit.Value);
            }

            catch(Exception ex)
            {
                _logger.LogError($"Error creating user: {ex.Message}");
                return Result<Unit>.Failure(new Error("UpdateUser.Failed", "An unexpected error occurred."));
            }           
        }
    }
}
