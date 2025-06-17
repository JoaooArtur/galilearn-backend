using Core.Shared.Errors;
using Core.Shared.Results;
using FluentAssertions;
using Moq;
using Serilog;
using Student.Application.Services;
using Student.Application.UseCases.Commands;
using Student.Domain.Entities;
using Student.Domain.Enumerations;
using Student.Shared.Commands;
using Xunit;

namespace Student.Tests.Application.Commands;

public class AcceptFriendRequestHandlerTests
{
    private readonly Mock<IStudentApplicationService> _applicationServiceMock;
    private readonly Mock<ILogger> _loggerMock;
    private readonly AcceptFriendRequestHandler _handler;

    public AcceptFriendRequestHandlerTests()
    {
        _applicationServiceMock = new Mock<IStudentApplicationService>();
        _loggerMock = new Mock<ILogger>();
        _handler = new AcceptFriendRequestHandler(_applicationServiceMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Succeed_When_Request_Is_Valid()
    {
        // Arrange
        var studentId = Guid.NewGuid();
        var requestId = Guid.NewGuid();
        var friendId = Guid.NewGuid();

        var student = Student.Domain.Aggregates.Student.Create(
            "Aluno Teste",
            "aluno@email.com",
            "senha123",
            "11999999999",
            DateTimeOffset.UtcNow.AddYears(-20)
        );

        typeof(Student.Domain.Aggregates.Student)
            .GetProperty(nameof(Student.Domain.Aggregates.Student.Id))?
            .SetValue(student, studentId);

        // Simula requisição pendente
        student.Requests.Add(FriendRequest.Create(requestId, friendId));

        var command = new AcceptFriendRequestCommand(studentId, requestId);

        _applicationServiceMock
            .Setup(x => x.LoadAggregateAsync<Student.Domain.Aggregates.Student>(studentId, It.IsAny<CancellationToken>()))
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
        var command = new AcceptFriendRequestCommand(Guid.NewGuid(), Guid.NewGuid());

        var expectedError = new Core.Shared.Errors.Error("StudentNotFound", "O estudante informado não foi encontrado.");

        _applicationServiceMock
            .Setup(x => x.LoadAggregateAsync<Student.Domain.Aggregates.Student>(command.StudentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure<Student.Domain.Aggregates.Student>(expectedError));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("StudentNotFound");
        result.Error.Message.Should().Be("O estudante informado não foi encontrado.");

        _applicationServiceMock.Verify(x =>
            x.AppendEventsAsync(It.IsAny<Student.Domain.Aggregates.Student>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_Should_Fail_When_Request_Already_Accepted()
    {
        // Arrange
        var studentId = Guid.NewGuid();
        var requestId = Guid.NewGuid();
        var friendId = Guid.NewGuid();

        var student = Student.Domain.Aggregates.Student.Create(
            "Aluno Teste",
            "aluno@email.com",
            "senha123",
            "11999999999",
            DateTimeOffset.UtcNow.AddYears(-20)
        );

        typeof(Student.Domain.Aggregates.Student)
            .GetProperty(nameof(Student.Domain.Aggregates.Student.Id))?
            .SetValue(student, studentId);

        // Simula requisição já aceita
        student.Requests.Add(FriendRequest.Create(requestId, friendId));
        student.AcceptFriendRequest(requestId, friendId);

        var command = new AcceptFriendRequestCommand(studentId, requestId);

        _applicationServiceMock
            .Setup(x => x.LoadAggregateAsync<Student.Domain.Aggregates.Student>(studentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(student));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("FriendAlreadyAdded");
        result.Error.Message.Should().Be("Vocês já são amigos!");

        _applicationServiceMock.Verify(x =>
            x.AppendEventsAsync(It.IsAny<Student.Domain.Aggregates.Student>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }
}
