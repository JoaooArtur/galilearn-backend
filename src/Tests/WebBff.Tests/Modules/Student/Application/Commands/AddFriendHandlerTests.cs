using Core.Shared.Errors;
using Core.Shared.Results;
using FluentAssertions;
using Moq;
using Serilog;
using Student.Application.Services;
using Student.Application.UseCases.Commands;
using Student.Shared.Commands;
using StudentAggregate = Student.Domain.Aggregates.Student;
using Xunit;

namespace WebBff.Tests.Modules.Student.Application.Commands;

public class AddFriendHandlerTests
{
    private readonly Mock<IStudentApplicationService> _applicationServiceMock;
    private readonly Mock<ILogger> _loggerMock;
    private readonly AddFriendHandler _handler;

    public AddFriendHandlerTests()
    {
        _applicationServiceMock = new Mock<IStudentApplicationService>();
        _loggerMock = new Mock<ILogger>();
        _handler = new AddFriendHandler(_applicationServiceMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Succeed_When_Friend_Not_Yet_Added()
    {
        // Arrange
        var studentId = Guid.NewGuid();
        var friendId = Guid.NewGuid();

        var student = StudentAggregate.Create(
            "Aluno Teste",
            "aluno@email.com",
            "senha123",
            "11999999999",
            DateTimeOffset.UtcNow.AddYears(-20)
        );

        typeof(StudentAggregate)
            .GetProperty(nameof(StudentAggregate.Id))?
            .SetValue(student, studentId);

        var command = new AddFriendCommand(studentId, friendId);

        _applicationServiceMock
            .Setup(x => x.LoadAggregateAsync<StudentAggregate>(studentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(student));

        _applicationServiceMock
            .Setup(x => x.AppendEventsAsync(student, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        _applicationServiceMock.Verify(x =>
            x.AppendEventsAsync(student, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Fail_When_Student_Not_Found()
    {
        // Arrange
        var command = new AddFriendCommand(Guid.NewGuid(), Guid.NewGuid());

        var expectedError = new Error("StudentNotFound", "O estudante informado não foi encontrado.");

        _applicationServiceMock
            .Setup(x => x.LoadAggregateAsync<StudentAggregate>(command.StudentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure<StudentAggregate>(expectedError));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("StudentNotFound");
        result.Error.Message.Should().Be("O estudante informado não foi encontrado.");

        _applicationServiceMock.Verify(x =>
            x.AppendEventsAsync(It.IsAny<StudentAggregate>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_Should_Fail_When_Friend_Already_Added()
    {
        // Arrange
        var studentId = Guid.NewGuid();
        var friendId = Guid.NewGuid();

        var student = StudentAggregate.Create(
            "Aluno Teste",
            "aluno@email.com",
            "senha123",
            "11999999999",
            DateTimeOffset.UtcNow.AddYears(-20)
        );

        typeof(StudentAggregate)
            .GetProperty(nameof(StudentAggregate.Id))?
            .SetValue(student, studentId);

        // Simula amizade já existente
        student.AddFriend(friendId);

        var command = new AddFriendCommand(studentId, friendId);

        _applicationServiceMock
            .Setup(x => x.LoadAggregateAsync<StudentAggregate>(studentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(student));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("FriendAlreadyAdded");
        result.Error.Message.Should().Be("Vocês já são amigos!");

        _applicationServiceMock.Verify(x =>
            x.AppendEventsAsync(It.IsAny<StudentAggregate>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }
}
