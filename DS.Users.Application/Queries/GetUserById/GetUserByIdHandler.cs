using DS.Domain.Entities;
using DS.Shared.Logging;
using DS.Shared.Results;
using DS.Users.Application.DTOs;
using DS.Users.Application.Interfaces;
using MediatR;

namespace DS.Users.Application.Queries.GetUserById
{
    public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, Result<UserDto>>
    {
        private readonly IBaseRepository<User> _repository;
        private readonly ILoggerManager _logger;

        public GetUserByIdHandler(IBaseRepository<User> repository, ILoggerManager logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<Result<UserDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _repository.GetByIdAsync(request.Id, cancellationToken);
                if (user == null)
                {
                    string message = $"User with ID {request.Id} not found.";
                    _logger.LogWarn(message);
                    return Result<UserDto>.Failure(new Error("GetUser.Failed", message));
                }

                var dto = new UserDto(
                    user.Id,
                    user.FirstName,
                    user.LastName,
                    user.Email,
                    user.CreatedAt,
                    user.LastModified
                    );
                return Result<UserDto>.Success(dto);
            }

            catch(Exception ex)
            {
                _logger.LogError($"Error creating user: {ex.Message}");
                return Result<UserDto>.Failure(new Error("GetUser.Failed", "An unexpected error occurred."));
            }
        }
    }
}
