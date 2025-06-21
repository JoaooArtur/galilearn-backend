using Core.Shared.Results;
using FluentAssertions;
using MediatR;
using Moq;
using Serilog;
using Student.Application.Services;
using Student.Application.UseCases.Commands;
using Student.Shared.Commands;
using Student.Shared.Response;
using Subject.Shared.Commands;
using StudentAggregate = Student.Domain.Aggregates.Student;
using Subject.Shared.Response;
using Xunit;

namespace WebBff.Tests.Modules.Student.Application.Commands;

public class AnswerAttemptHandlerTests
{
    private readonly Mock<IStudentApplicationService> _applicationServiceMock;
    private readonly Mock<ISender> _senderMock;
    private readonly AnswerAttemptHandler _handler;

    public AnswerAttemptHandlerTests()
    {
        _applicationServiceMock = new Mock<IStudentApplicationService>();
        _senderMock = new Mock<ISender>();

        _handler = new AnswerAttemptHandler(
            _applicationServiceMock.Object,
            _senderMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Succeed_When_AnswerIsCorrect()
    {
        // Arrange
        var studentId = Guid.NewGuid();
        var subjectId = Guid.NewGuid();
        var lessonId = Guid.NewGuid();
        var attemptId = Guid.NewGuid();
        var questionId = Guid.NewGuid();
        var answerId = Guid.NewGuid();
        var correctAnswerId = Guid.NewGuid();

        var command = new AnswerAttemptCommand(studentId, subjectId, lessonId, attemptId, questionId, answerId);

        var student = StudentAggregate.Create(
            name: "Aluno Teste",
            email: "aluno@email.com",
            password: "senha123",
            phone: "11999999999",
            dateOfBirth: DateTimeOffset.UtcNow.AddYears(-20)
        );

        typeof(StudentAggregate)
            .GetProperty(nameof(StudentAggregate.Id))?
            .SetValue(student, studentId);

        _applicationServiceMock
            .Setup(x => x.LoadAggregateAsync<StudentAggregate>(studentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(student));

        _senderMock
            .Setup(x => x.Send(It.IsAny<CheckCorrectAnswerCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(new CheckCorrectAnswerResponse(correctAnswerId, true)));

        _applicationServiceMock
            .Setup(x => x.AppendEventsAsync(student, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.AttemptId.Should().Be(attemptId);
        result.Value.QuestionId.Should().Be(questionId);
        result.Value.AnswerId.Should().Be(answerId);
        result.Value.CorrectAnswerId.Should().Be(correctAnswerId);
        result.Value.IsCorrect.Should().BeTrue();

        _applicationServiceMock.Verify(x =>
            x.AppendEventsAsync(student, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Fail_When_Student_Not_Found()
    {
        // Arrange
        var command = new AnswerAttemptCommand(
            StudentId: Guid.NewGuid(),
            SubjectId: Guid.NewGuid(),
            LessonId: Guid.NewGuid(),
            AttemptId: Guid.NewGuid(),
            QuestionId: Guid.NewGuid(),
            AnswerId: Guid.NewGuid()
        );

        var expectedError = new Core.Shared.Errors.Error("StudentNotFound", "O estudante informado não foi encontrado.");

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
    public async Task Handle_Should_Fail_When_CheckAnswer_Fails()
    {
        // Arrange
        var studentId = Guid.NewGuid();
        var subjectId = Guid.NewGuid();
        var lessonId = Guid.NewGuid();
        var attemptId = Guid.NewGuid();
        var questionId = Guid.NewGuid();
        var answerId = Guid.NewGuid();

        var command = new AnswerAttemptCommand(studentId, subjectId, lessonId, attemptId, questionId, answerId);

        var student = StudentAggregate.Create(
            name: "Aluno Teste",
            email: "aluno@email.com",
            password: "senha123",
            phone: "11999999999",
            dateOfBirth: DateTimeOffset.UtcNow.AddYears(-20)
        );

        typeof(StudentAggregate)
            .GetProperty(nameof(StudentAggregate.Id))?
            .SetValue(student, studentId);

        var expectedError = new Core.Shared.Errors.Error("CheckFailed", "Erro ao verificar a resposta correta.");

        _applicationServiceMock
            .Setup(x => x.LoadAggregateAsync<StudentAggregate>(studentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(student));

        _senderMock
            .Setup(x => x.Send(It.IsAny<CheckCorrectAnswerCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure<CheckCorrectAnswerResponse>(expectedError));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("CheckFailed");
        result.Error.Message.Should().Be("Erro ao verificar a resposta correta.");

        _applicationServiceMock.Verify(x =>
            x.AppendEventsAsync(It.IsAny<StudentAggregate>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }
}
