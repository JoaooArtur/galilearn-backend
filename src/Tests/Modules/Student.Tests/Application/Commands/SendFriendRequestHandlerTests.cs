using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Shared.Errors;
using Core.Shared.Results;
using Moq;
using Serilog;
using Student.Application.Services;
using Student.Application.UseCases.Commands;
using StudentAggregate = Student.Domain.Aggregates.Student;
using Student.Domain.Entities;
using Student.Shared.Commands;
using Xunit;
using static Student.Domain.Dto;

namespace Student.Tests.Application.Commands
{
    public class SendFriendRequestHandlerTests
    {
        private readonly Mock<IStudentApplicationService> _applicationServiceMock;
        private readonly Mock<ILogger> _loggerMock;
        private readonly SendFriendRequestHandler _handler;

        public SendFriendRequestHandlerTests()
        {
            _applicationServiceMock = new Mock<IStudentApplicationService>();
            _loggerMock = new Mock<ILogger>();

            _handler = new SendFriendRequestHandler(
                _applicationServiceMock.Object,
                _loggerMock.Object
            );
        }

        [Fact(DisplayName = "Deve enviar solicitação de amizade com sucesso")]
        public async Task Handle_ShouldSendFriendRequest_WhenValidData()
        {
            // Arrange
            var studentId = Guid.NewGuid();
            var friendId = Guid.NewGuid();

            var friend = StudentAggregate.Create(
                "Friend Name",
                "friend@email.com",
                "password",
                "11999999999",
                DateTimeOffset.UtcNow.AddYears(-20)
            );

            _applicationServiceMock.Setup(x =>
                x.LoadAggregateAsync<StudentAggregate>(friendId, It.IsAny<CancellationToken>())
            ).ReturnsAsync(Result.Success(friend));

            var command = new SendFriendRequestCommand(studentId, friendId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            _applicationServiceMock.Verify(x =>
                x.AppendEventsAsync(friend, It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact(DisplayName = "Deve falhar se amigo não encontrado")]
        public async Task Handle_ShouldFail_WhenFriendNotFound()
        {
            // Arrange
            var studentId = Guid.NewGuid();
            var friendId = Guid.NewGuid();

            _applicationServiceMock.Setup(x =>
                x.LoadAggregateAsync<StudentAggregate>(friendId, It.IsAny<CancellationToken>())
            ).ReturnsAsync(Result.Failure<StudentAggregate>(new Error("NotFound", "Amigo não encontrado")));

            var command = new SendFriendRequestCommand(studentId, friendId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal("NotFound", result.Error.Code);
            Assert.Equal("Amigo não encontrado", result.Error.Message);

            _applicationServiceMock.Verify(x =>
                x.AppendEventsAsync(It.IsAny<StudentAggregate>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact(DisplayName = "Deve falhar se já são amigos")]
        public async Task Handle_ShouldFail_WhenAlreadyFriends()
        {
            // Arrange
            var studentId = Guid.NewGuid();
            var friendId = Guid.NewGuid();

            var friend = StudentAggregate.Create(
                "Friend Name",
                "friend@email.com",
                "password",
                "11999999999",
                DateTimeOffset.UtcNow.AddYears(-20)
            );

            // Simula que já são amigos
            friend.Friends.Add(new Friend(studentId));

            _applicationServiceMock.Setup(x =>
                x.LoadAggregateAsync<StudentAggregate>(friendId, It.IsAny<CancellationToken>())
            ).ReturnsAsync(Result.Success(friend));

            var command = new SendFriendRequestCommand(studentId, friendId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal("FriendAlreadyAdded", result.Error.Code);
            Assert.Equal("Vocês já são amigos!", result.Error.Message);

            _applicationServiceMock.Verify(x =>
                x.AppendEventsAsync(It.IsAny<StudentAggregate>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }
    }
}
