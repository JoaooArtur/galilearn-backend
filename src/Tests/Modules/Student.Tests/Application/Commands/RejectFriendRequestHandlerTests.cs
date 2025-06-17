using FluentAssertions;
using Moq;
using Serilog;
using Student.Application.Services;
using Student.Application.UseCases.Commands;
using StudentAggregate = Student.Domain.Aggregates.Student;
using Student.Shared.Commands;
using Core.Shared.Results;
using Student.Domain.Entities;

namespace Student.Tests.Application.Commands;

public class RejectFriendRequestHandlerTests
{
    private readonly Mock<IStudentApplicationService> _applicationServiceMock;
    private readonly Mock<ILogger> _loggerMock;
    private readonly RejectFriendRequestHandler _handler;

    public RejectFriendRequestHandlerTests()
    {
        _applicationServiceMock = new Mock<IStudentApplicationService>();
        _loggerMock = new Mock<ILogger>();

        _handler = new RejectFriendRequestHandler(
            _applicationServiceMock.Object,
            _loggerMock.Object);
    }
    [Fact]
    public async Task Handle_Should_Succeed_When_Aggregate_Loaded()
    {
        // Arrange
        var command = new RejectFriendRequestCommand(
            StudentId: Guid.NewGuid(),
            RequestId: Guid.NewGuid()
        );

        var student = StudentAggregate.Create(
            name: "Aluno Teste",
            email: "aluno@email.com",
            password: "senha123",
            phone: "11988887777",
            dateOfBirth: DateTimeOffset.UtcNow.AddYears(-20)
        );

        typeof(StudentAggregate)
            .GetProperty(nameof(StudentAggregate.Id))?
            .SetValue(student, command.StudentId);
        typeof(StudentAggregate)
            .GetProperty(nameof(StudentAggregate.Requests))?
            .SetValue(student, new List<FriendRequest>());

        _applicationServiceMock
            .Setup(x => x.LoadAggregateAsync<StudentAggregate>(command.StudentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(student));

        _applicationServiceMock
            .Setup(x => x.AppendEventsAsync(student, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        _applicationServiceMock.Verify(x =>
            x.AppendEventsAsync(It.IsAny<StudentAggregate>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Fail_When_Aggregate_NotFound()
    {
        // Arrange
        var command = new RejectFriendRequestCommand(
            StudentId: Guid.NewGuid(),
            RequestId: Guid.NewGuid()
        );

        var expectedError = new Core.Shared.Errors.Error(
            code: "StudentNotFound",
            message: "O estudante informado não foi encontrado."
        );

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
}
