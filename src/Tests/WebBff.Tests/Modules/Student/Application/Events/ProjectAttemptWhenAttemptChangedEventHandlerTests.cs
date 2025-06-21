using MediatR;
using MongoDB.Driver;
using Moq;
using Student.Application.Services;
using Student.Application.UseCases.Events;
using Student.Domain.Enumerations;
using Student.Domain;
using Student.Persistence.Projections;
using Serilog;
using System.Linq.Expressions;
using StudentAggregate = Student.Domain.Aggregates.Student;
using Core.Shared.Results;

namespace WebBff.Tests.Modules.Student.Application.Events;
public class ProjectAttemptWhenAttemptChangedEventHandlerTests
{
    private readonly Mock<IStudentProjection<Projection.Attempt>> _attemptProjectionMock = new();
    private readonly Mock<IStudentProjection<Projection.LessonProgress>> _lessonProjectionMock = new();
    private readonly Mock<IStudentApplicationService> _applicationServiceMock = new();
    private readonly Mock<ILogger> _loggerMock = new();

    private readonly ProjectAttemptWhenAttemptChangedEventHandler _handler;

    public ProjectAttemptWhenAttemptChangedEventHandlerTests()
    {
        _handler = new ProjectAttemptWhenAttemptChangedEventHandler(
            _attemptProjectionMock.Object,
            _lessonProjectionMock.Object,
            _applicationServiceMock.Object,
            _loggerMock.Object
        );
    }

    [Fact]
    public async Task Handle_Should_Insert_Attempt_When_AttemptCreated()
    {
        var @event = new DomainEvent.AttemptCreated(
            Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 1);

        _attemptProjectionMock.Setup(x => x.ReplaceInsertAsync(It.IsAny<Projection.Attempt>(), It.IsAny<CancellationToken>()))
            .Returns(ValueTask.CompletedTask);

        await _handler.Handle(@event);

        _attemptProjectionMock.Verify(x => x.ReplaceInsertAsync(It.Is<Projection.Attempt>(a =>
            a.Id == @event.AttemptId &&
            a.StudentId == @event.StudentId &&
            a.LessonId == @event.LessonId &&
            a.Status == AttemptStatus.Pending), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Update_Attempt_When_AttemptAnswered_Correct()
    {
        var attemptId = Guid.NewGuid();
        var studentId = Guid.NewGuid();
        var lessonId = Guid.NewGuid();
        var subjectId = Guid.NewGuid();
        var questionId = Guid.NewGuid();
        var answerId = Guid.NewGuid();

        var @event = new DomainEvent.AttemptAnswered(
            attemptId, studentId, subjectId, lessonId, questionId, answerId, true, 2);

        var attempt = new Projection.Attempt(attemptId, studentId, lessonId, 1, 1, AttemptStatus.Pending, DateTimeOffset.Now);

        _attemptProjectionMock.Setup(x => x.GetAsync(attemptId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(attempt);

        _lessonProjectionMock.Setup(x => x.FindAsync(It.IsAny<Expression<Func<Projection.LessonProgress, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Projection.LessonProgress(Guid.NewGuid(), subjectId, lessonId, studentId, LessonStatus.InProgress, DateTimeOffset.UtcNow));

        var updateDefinition = Builders<Projection.Attempt>.Update
            .Set(a => a.QuestionsAnswered, 2)
            .Set(a => a.CorrectAnswers, 2);

        var collectionMock = new Mock<IMongoCollection<Projection.Attempt>>();
        _attemptProjectionMock.Setup(x => x.GetCollection()).Returns(collectionMock.Object);

        collectionMock.Setup(x =>
            x.UpdateOneAsync(It.IsAny<FilterDefinition<Projection.Attempt>>(),
                             It.IsAny<UpdateDefinition<Projection.Attempt>>(),
                             null,
                             It.IsAny<CancellationToken>()))
            .ReturnsAsync(Mock.Of<UpdateResult>());

        await _handler.Handle(@event, CancellationToken.None);

        _attemptProjectionMock.Verify(x => x.GetAsync(attemptId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Update_Status_When_AttemptInProgressStatus()
    {
        var attemptId = Guid.NewGuid();
        var subjectId = Guid.NewGuid();
        var lessonId = Guid.NewGuid();
        var status = AttemptStatus.Pending;

        var @event = new DomainEvent.AttemptInProgressStatus(attemptId, lessonId, subjectId, status, 2);
        _attemptProjectionMock
            .Setup(x => x.UpdateOneFieldAsync(
                attemptId,
                It.IsAny<Expression<Func<Projection.Attempt, AttemptStatus>>>(),
                status,
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        await _handler.Handle(@event, CancellationToken.None);
        _attemptProjectionMock.Verify(x =>
            x.UpdateOneFieldAsync(
                attemptId,
                It.IsAny<Expression<Func<Projection.Attempt, string>>>(),
                AttemptStatus.Pending.ToString(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Finish_Lesson_When_AttemptFinishedStatus_And_5CorrectAnswers()
    {
        var attemptId = Guid.NewGuid();
        var studentId = Guid.NewGuid();
        var lessonId = Guid.NewGuid();
        var subjectId = Guid.NewGuid();

        var @event = new DomainEvent.AttemptFinishedStatus(
            attemptId, lessonId, subjectId, AttemptStatus.Finished, 4);

        var attempt = new Projection.Attempt(
            attemptId, studentId, lessonId, 6, 5, AttemptStatus.Pending, DateTimeOffset.Now);

        var student = StudentAggregate.Create(
            "Aluno Teste", "teste@email.com", "senha", "11999999999", DateTimeOffset.UtcNow.AddYears(-20));

        typeof(StudentAggregate)
            .GetProperty(nameof(StudentAggregate.Id))?
            .SetValue(student, studentId);

        _attemptProjectionMock.Setup(x => x.GetAsync(attemptId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(attempt);

        _applicationServiceMock.Setup(x => x.LoadAggregateAsync<StudentAggregate>(
            studentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(student));

        _applicationServiceMock.Setup(x => x.AppendEventsAsync(student, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _attemptProjectionMock.Setup(x => x.UpdateOneFieldAsync(
            attemptId,
            It.IsAny<Expression<Func<Projection.Attempt, string>>>(),
            AttemptStatus.Finished.ToString(),
            It.IsAny<CancellationToken>()))
        .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(@event, CancellationToken.None);

        // Assert
        _attemptProjectionMock.Verify(x => x.UpdateOneFieldAsync(
            attemptId,
            It.IsAny<Expression<Func<Projection.Attempt, string>>>(),
            AttemptStatus.Finished.ToString(),
            It.IsAny<CancellationToken>()), Times.Once);

        _applicationServiceMock.Verify(x =>
            x.AppendEventsAsync(student, It.IsAny<CancellationToken>()),
            Times.AtLeastOnce);
    }

}
