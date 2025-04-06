using DS.Domain.Entities;
using DS.Users.Application.Commands.UpdateUser;
using DS.Users.Application.Interfaces;
using Moq;


namespace DS.Users.Tests.UserTests
{
    public class UpdateUserCommandHandlerTests
    {
        private readonly Mock<IBaseRepository<User>> _userRepositoryMock;
        private readonly UpdateUserHandler _handler;
        private readonly Mock<Shared.Logging.ILoggerManager> _logger;

        public UpdateUserCommandHandlerTests()
        {
            _userRepositoryMock = new Mock<IBaseRepository<User>>();
            _logger = new Mock<Shared.Logging.ILoggerManager>();
            _handler = new UpdateUserHandler(_userRepositoryMock.Object, _logger.Object);
        }

        [Fact]
        public async Task Handle_ShouldUpdateUser_WhenUserExists()
        {
            var userId = Guid.NewGuid();
            var existingUser = new User { Id = userId, FirstName = "John" };

            _userRepositoryMock.Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingUser);
            _userRepositoryMock.Setup(r => r.UpdateAsync(existingUser))
                .Returns(Task.CompletedTask);

            var command = new UpdateUserCommand { Id = userId, FirstName = "Cian" };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result.IsSuccess);
            _userRepositoryMock.Verify(r => r.UpdateAsync(existingUser), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenUserNotFound()
        {
            var userId = Guid.NewGuid();
            _userRepositoryMock.Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((User)null);

            var command = new UpdateUserCommand { Id = userId, FirstName = "AAA" };
            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
        }
    }
}
