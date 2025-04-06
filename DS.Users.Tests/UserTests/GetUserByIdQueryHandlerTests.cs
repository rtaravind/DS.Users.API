using DS.Domain.Entities;
using DS.Users.Application.Interfaces;
using DS.Users.Application.Queries.GetUserById;
using Moq;

namespace DS.Users.Tests.UserTests
{
    public class GetUserByIdQueryHandlerTests
    {
        private readonly Mock<IBaseRepository<User>> _userRepositoryMock;
        private readonly GetUserByIdHandler _handler;
        private readonly Mock<Shared.Logging.ILoggerManager> _logger;

        public GetUserByIdQueryHandlerTests()
        {
            _userRepositoryMock = new Mock<IBaseRepository<User>>();
            _logger = new Mock<Shared.Logging.ILoggerManager>();
            _handler = new GetUserByIdHandler(_userRepositoryMock.Object, _logger.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnUserDto_WhenUserExists()
        {
            var userId = Guid.NewGuid();
            var query = new GetUserByIdQuery { Id = userId };
            var user = new User { Id = userId, FirstName = "John", LastName = "Smith", Email = "john@example.com" };

            _userRepositoryMock.Setup(repo => repo.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.Equal(userId, result.Value.Id);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenUserNotFound()
        {
            var userId = Guid.NewGuid();
            var query = new GetUserByIdQuery { Id = userId };
            _userRepositoryMock.Setup(repo => repo.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((User)null);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Equal("User not found", result.Error.Message);
        }

    }
}
