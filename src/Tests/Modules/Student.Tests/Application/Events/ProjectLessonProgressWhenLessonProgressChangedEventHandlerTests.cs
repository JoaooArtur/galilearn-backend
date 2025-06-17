using System.Linq.Expressions;
using Core.Shared.Results;
using FluentAssertions;
using Moq;
using Serilog;
using Student.Application.UseCases.Events;
using Student.Domain;
using Student.Domain.Enumerations;
using Student.Persistence.Projections;
using Student.Shared.Commands;
using Subject.Shared.Queries;
using Xunit;
using MediatR;
using Student.Application.Services;
using Subject.Shared.Response;

namespace Student.Tests.Application.Events;

public class ProjectLessonProgressWhenLessonProgressChangedEventHandlerTests
{
    private readonly Mock<IStudentProjection<Projection.LessonProgress>> _lessonProgressProjectionMock = new();
    private readonly Mock<IStudentApplicationService> _applicationServiceMock = new();
    private readonly Mock<ISender> _senderMock = new();
    private readonly Mock<ILogger> _loggerMock = new();

    private readonly ProjectLessonProgressWhenLessonProgressChangedEventHandler _handler;

    public ProjectLessonProgressWhenLessonProgressChangedEventHandlerTests()
    {
        _handler = new ProjectLessonProgressWhenLessonProgressChangedEventHandler(
            _lessonProgressProjectionMock.Object,
            _applicationServiceMock.Object,
            _senderMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Create_LessonProgress_When_Created()
    {
        var @event = new DomainEvent.LessonProgressCreated(
            Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 1);

        _lessonProgressProjectionMock
            .Setup(x => x.ReplaceInsertAsync(It.IsAny<Projection.LessonProgress>(), It.IsAny<CancellationToken>()))
            .Returns(ValueTask.CompletedTask);

        await _handler.Handle(@event);

        _lessonProgressProjectionMock.Verify(x =>
            x.ReplaceInsertAsync(It.Is<Projection.LessonProgress>(l =>
                l.Id == @event.Id &&
                l.SubjectId == @event.SubjectId &&
                l.LessonId == @event.LessonId &&
                l.StudentId == @event.StudentId &&
                l.Status == LessonStatus.InProgress),
                It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Update_Status_When_LessonFinished()
    {
        var subjectId = Guid.NewGuid();
        var lessonId = Guid.NewGuid();
        var studentId = Guid.NewGuid();
        var projectionId = Guid.NewGuid();

        var @event = new DomainEvent.LessonProgressFinishedStatus(lessonId, subjectId, studentId, LessonStatus.Finished, 2);

        var progress = new Projection.LessonProgress(projectionId, subjectId , lessonId, studentId, LessonStatus.InProgress, DateTimeOffset.Now)
        {
            Id = projectionId
        };

        _lessonProgressProjectionMock
            .Setup(x => x.FindAsync(It.IsAny<Expression<Func<Projection.LessonProgress, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(progress);

        _lessonProgressProjectionMock
            .Setup(x => x.UpdateOneFieldAsync(
                projectionId,
                It.IsAny<Expression<Func<Projection.LessonProgress, string>>>(),
                LessonStatus.Finished.ToString(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _lessonProgressProjectionMock
            .Setup(x => x.ListAsync(It.IsAny<Expression<Func<Projection.LessonProgress, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([progress]);

        _senderMock
            .Setup(x => x.Send(It.IsAny<GetLessonCountBySubjectIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(new CountResponse(1)));

        var student = Student.Domain.Aggregates.Student.Create("Aluno", "email@email.com", "senha", "11999999999", DateTimeOffset.UtcNow.AddYears(-20));

        typeof(Student.Domain.Aggregates.Student)
            .GetProperty(nameof(Student.Domain.Aggregates.Student.Id))?
            .SetValue(student, studentId);

        _applicationServiceMock
            .Setup(x => x.LoadAggregateAsync<Student.Domain.Aggregates.Student>(studentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(student));

        _applicationServiceMock
            .Setup(x => x.AppendEventsAsync(student, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        await _handler.Handle(@event);

        _lessonProgressProjectionMock.Verify(x =>
            x.UpdateOneFieldAsync(
                projectionId,
                It.IsAny<Expression<Func<Projection.LessonProgress, string>>>(),
                LessonStatus.Finished.ToString(),
                It.IsAny<CancellationToken>()), Times.Once);

        _applicationServiceMock.Verify(x =>
            x.AppendEventsAsync(student, It.IsAny<CancellationToken>()),
            Times.Exactly(2)); // 1x status + 1x xp
    }

    [Fact]
    public async Task Handle_Should_Not_Change_Subject_When_Lessons_Remaining()
    {
        var @event = new DomainEvent.LessonProgressFinishedStatus(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), LessonStatus.Finished, 2);

        _lessonProgressProjectionMock
            .Setup(x => x.FindAsync(It.IsAny<Expression<Func<Projection.LessonProgress, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Projection.LessonProgress(Guid.NewGuid(), @event.SubjectId, @event.LessonId, @event.StudentId, LessonStatus.InProgress, DateTimeOffset.Now) { Id = Guid.NewGuid() });

        _lessonProgressProjectionMock
            .Setup(x => x.UpdateOneFieldAsync(It.IsAny<Guid>(), It.IsAny<Expression<Func<Projection.LessonProgress, string>>>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _lessonProgressProjectionMock
            .Setup(x => x.ListAsync(It.IsAny<Expression<Func<Projection.LessonProgress, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([]); // nenhuma lição finalizada

        _senderMock
            .Setup(x => x.Send(It.IsAny<GetLessonCountBySubjectIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(new CountResponse(2)));

        await _handler.Handle(@event);

        _applicationServiceMock.Verify(x =>
            x.AppendEventsAsync(It.IsAny<Student.Domain.Aggregates.Student>(), It.IsAny<CancellationToken>()),
            Times.Never); // não deveria ser chamado
    }
}
