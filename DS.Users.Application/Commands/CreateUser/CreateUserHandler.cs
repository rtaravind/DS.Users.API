using DS.Domain.Entities;
using DS.Shared.Logging;
using DS.Shared.Results;
using DS.Users.Application.Interfaces;
using FluentValidation;
using MediatR;

namespace DS.Users.Application.Commands.CreateUser
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, Result<Guid>>
    {
        private readonly IBaseRepository<User> _repository;
        private readonly IValidator<CreateUserCommand> _validator;
        private readonly ILoggerManager _logger;

        public CreateUserHandler(
            IBaseRepository<User> repository,
            IValidator<CreateUserCommand> validator,
            ILoggerManager logger)
        {
            _repository = repository;
            _validator = validator;
            _logger = logger;
        }

        public async Task<Result<Guid>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                return Result<Guid>.Failure(new Error("CreateUser.Failed", validationResult.ToString()));
            }

            try
            {
                var user = new User
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    CreatedAt = DateTime.UtcNow,
                    LastModified = DateTime.UtcNow
                };

                await _repository.AddAsync(user, cancellationToken);
                await _repository.SaveChangesAsync(cancellationToken);
                _logger.LogInfo($"User created with ID {user.Id}");

                return Result<Guid>.Success(user.Id);
            }
            
            catch(Exception ex)
            {
                _logger.LogError($"Error creating user: {ex.Message}");
                return Result<Guid>.Failure(new Error("CreateUser.Failed", "An unexpected error occurred."));
            }
            
        }
    }
}
