using FluentAssertions;
using Moq;
using Serilog;
using Student.Application.Services;
using Student.Application.UseCases.Commands;
using Student.Shared.Commands;
using StudentAggregate = Student.Domain.Aggregates.Student;
using Student.Shared.Response;
using Core.Shared.Results;
using Xunit;

namespace WebBff.Tests.Modules.Student.Application.Commands;

public class CreateAttemptHandlerTests
{
    private readonly Mock<IStudentApplicationService> _applicationServiceMock;
    private readonly Mock<ILogger> _loggerMock;
    private readonly CreateAttemptHandler _handler;

    public CreateAttemptHandlerTests()
    {
        _applicationServiceMock = new Mock<IStudentApplicationService>();
        _loggerMock = new Mock<ILogger>();

        _handler = new CreateAttemptHandler(
            _applicationServiceMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Succeed_When_AllProgressIsAdded()
    {
        // Arrange
        var studentId = Guid.NewGuid();
        var subjectId = Guid.NewGuid();
        var lessonId = Guid.NewGuid();

        var student = StudentAggregate.Create(
            name: "Aluno Teste",
            email: "aluno@email.com",
            password: "senha123",
            phone: "11999999999",
            dateOfBirth: DateTimeOffset.UtcNow.AddYears(-20)
        );

        // O ID precisa bater com o do comando
        typeof(StudentAggregate)
            .GetProperty(nameof(StudentAggregate.Id))?
            .SetValue(student, studentId);

        var command = new CreateAttemptCommand(studentId, subjectId, lessonId);

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
        result.Value.Should().NotBeNull();
        result.Value.Id.Should().NotBeEmpty();

        _applicationServiceMock.Verify(x =>
            x.AppendEventsAsync(It.IsAny<StudentAggregate>(), It.IsAny<CancellationToken>()),
            Times.Exactly(3)); // AddSubjectProgress + AddLessonProgress + AddAttempt
    }

    [Fact]
    public async Task Handle_Should_Fail_When_Aggregate_NotFound()
    {
        // Arrange
        var command = new CreateAttemptCommand(
            StudentId: Guid.NewGuid(),
            SubjectId: Guid.NewGuid(),
            LessonId: Guid.NewGuid()
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
