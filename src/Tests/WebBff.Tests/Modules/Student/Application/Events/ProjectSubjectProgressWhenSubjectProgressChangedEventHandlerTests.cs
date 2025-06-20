using System.Linq.Expressions;
using FluentAssertions;
using Moq;
using Serilog;
using Student.Application.UseCases.Events;
using Student.Domain;
using Student.Domain.Enumerations;
using Student.Persistence.Projections;
using Xunit;
using MediatR;
using Student.Application.Services;

namespace WebBff.Tests.Modules.Student.Application.Events;

public class ProjectSubjectProgressWhenSubjectProgressChangedEventHandlerTests
{
    private readonly Mock<IStudentProjection<Projection.SubjectProgress>> _subjectProgressProjectionMock = new();
    private readonly Mock<IStudentApplicationService> _applicationServiceMock = new();
    private readonly Mock<ISender> _senderMock = new();
    private readonly Mock<ILogger> _loggerMock = new();

    private readonly ProjectSubjectProgressWhenSubjectProgressChangedEventHandler _handler;

    public ProjectSubjectProgressWhenSubjectProgressChangedEventHandlerTests()
    {
        _handler = new ProjectSubjectProgressWhenSubjectProgressChangedEventHandler(
            _subjectProgressProjectionMock.Object,
            _applicationServiceMock.Object,
            _senderMock.Object,
            _loggerMock.Object
        );
    }

    [Fact]
    public async Task Handle_Should_Insert_SubjectProgress_When_Created()
    {
        var @event = new DomainEvent.SubjectProgressCreated(
            Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 1);

        _subjectProgressProjectionMock
            .Setup(x => x.ReplaceInsertAsync(It.IsAny<Projection.SubjectProgress>(), It.IsAny<CancellationToken>()))
            .Returns(ValueTask.CompletedTask);

        await _handler.Handle(@event);

        _subjectProgressProjectionMock.Verify(x =>
            x.ReplaceInsertAsync(It.Is<Projection.SubjectProgress>(s =>
                s.Id == @event.Id &&
                s.SubjectId == @event.SubjectId &&
                s.StudentId == @event.StudentId &&
                s.Status == LessonStatus.InProgress),
                It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Update_Status_When_Finished()
    {
        var studentId = Guid.NewGuid();
        var subjectId = Guid.NewGuid();
        var progressId = Guid.NewGuid();

        var @event = new DomainEvent.SubjectProgressFinishedStatus(
            subjectId, studentId, LessonStatus.Finished, 2);

        var projection = new Projection.SubjectProgress(
            progressId, subjectId, studentId, LessonStatus.InProgress, DateTimeOffset.Now);

        _subjectProgressProjectionMock
            .Setup(x => x.FindAsync(It.IsAny<Expression<Func<Projection.SubjectProgress, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(projection);

        _subjectProgressProjectionMock
            .Setup(x => x.UpdateOneFieldAsync(
                progressId,
                It.IsAny<Expression<Func<Projection.SubjectProgress, string>>>(),
                LessonStatus.Finished.ToString(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        await _handler.Handle(@event);

        _subjectProgressProjectionMock.Verify(x =>
            x.UpdateOneFieldAsync(
                progressId,
                It.IsAny<Expression<Func<Projection.SubjectProgress, string>>>(),
                LessonStatus.Finished.ToString(),
                It.IsAny<CancellationToken>()), Times.Once);
    }
}
