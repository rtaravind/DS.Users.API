using DS.Domain.Entities;
using DS.Users.Application.Commands.DeleteUser;
using DS.Users.Application.Interfaces;
using DS.Users.Application.Queries.GetUserById;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS.Users.Tests.UserTests
{
    public class DeleteUserCommandHandlerTests
    {
        private readonly Mock<IBaseRepository<User>> _userRepositoryMock;
        private readonly DeleteUserHandler _handler;
        private readonly Mock<Shared.Logging.ILoggerManager> _logger;

        public DeleteUserCommandHandlerTests()
        {
            _userRepositoryMock = new Mock<IBaseRepository<User>>();
            _logger = new Mock<Shared.Logging.ILoggerManager>();
            _handler = new DeleteUserHandler(_userRepositoryMock.Object, _logger.Object);
        }

        [Fact]
        public async Task Handle_ShouldDeleteUser_WhenUserExists()
        {
            var userId = Guid.NewGuid();
            var query = new DeleteUserCommand { Id = userId };
            var user = new User { Id = userId };

            _userRepositoryMock.Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);
            _userRepositoryMock.Setup(r => r.DeleteAsync(user)).Returns(Task.CompletedTask);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.True(result.IsSuccess);
            _userRepositoryMock.Verify(r => r.DeleteAsync(user), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenUserNotFound()
        {
            var userId = Guid.NewGuid();
            var query = new DeleteUserCommand { Id = userId };
            _userRepositoryMock.Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((User)null);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.False(result.IsSuccess);
        }
    }
}
