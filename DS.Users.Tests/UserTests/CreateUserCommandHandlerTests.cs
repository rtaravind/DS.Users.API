using DS.Domain.Entities;
using DS.Users.Application.Commands.CreateUser;
using DS.Users.Application.Interfaces;
using FluentValidation;
using Moq;


namespace DS.Users.Tests.UserTests
{
    public class CreateUserCommandHandlerTests
    {
        private readonly Mock<IBaseRepository<User>> _userRepositoryMock;
        private readonly Mock<Shared.Logging.ILoggerManager> _logger;
        private readonly Mock<IValidator<CreateUserCommand>> _validatorMock;
        private readonly CreateUserHandler _handler;

        public CreateUserCommandHandlerTests()
        {
            _userRepositoryMock = new Mock<IBaseRepository<User>>();
            _logger = new Mock<Shared.Logging.ILoggerManager>();
            _validatorMock = new Mock<IValidator<CreateUserCommand>>();

            _handler = new CreateUserHandler(_userRepositoryMock.Object, _validatorMock.Object, _logger.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnUserId_WhenUserIsCreated()
        {
            var command = new CreateUserCommand { FirstName = "Test User", LastName = "Test User", Email = "test@example.com" };
            var userId = Guid.NewGuid();

            _userRepositoryMock
                .Setup(repo => repo.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                .Callback<User, CancellationToken>((user, token) =>
                {
                    user.Id = userId;
                })
                .Returns(Task.CompletedTask);

            _validatorMock
                .Setup(v => v.ValidateAsync(It.IsAny<CreateUserCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());


            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.Equal(userId, result.Value);
            _userRepositoryMock.Verify(r => r.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldFail_WhenFirstNameIsMissing()
        {
            var command = new CreateUserCommand
            {
                FirstName = "",
                LastName = "Test",
                Email = "test@example.com"
            };

            _validatorMock
                .Setup(v => v.ValidateAsync(It.IsAny<CreateUserCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult
                {
                    Errors = new List<FluentValidation.Results.ValidationFailure>
                      {
                       new FluentValidation.Results.ValidationFailure("FirstName", "FirstName is required")
                      }
                });

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Contains("FirstName is required", result.Error.Message);
        }
    }
}
