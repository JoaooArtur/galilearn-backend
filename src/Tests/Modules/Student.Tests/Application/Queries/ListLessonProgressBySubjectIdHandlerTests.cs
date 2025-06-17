using Core.Shared.Results;
using FluentAssertions;
using Moq;
using Student.Application.UseCases.Queries;
using Student.Domain.Enumerations;
using Student.Persistence.Projections;
using Student.Shared.Queries;
using Student.Shared.Response;
using Subject.Shared.Queries;
using Subject.Shared.Response;
using Xunit;
using Student.Domain;
using MediatR;
using System.Linq.Expressions;

namespace Student.Tests.Application.Queries;

public class ListLessonProgressBySubjectIdHandlerTests
{
    private readonly Mock<IStudentProjection<Projection.LessonProgress>> _projectionMock = new();
    private readonly Mock<ISender> _senderMock = new();
    private readonly ListLessonProgressBySubjectIdHandler _handler;

    public ListLessonProgressBySubjectIdHandlerTests()
    {
        _handler = new ListLessonProgressBySubjectIdHandler(_projectionMock.Object, _senderMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Return_LessonProgress_With_Existing_Status()
    {
        // Arrange
        var studentId = Guid.NewGuid();
        var subjectId = Guid.NewGuid();
        var lessonId = Guid.NewGuid();

        var lesson = new PagedLessonBySubjectIdResponse(
            lessonId, subjectId, "Título", "Conteúdo", 5, 1, DateTimeOffset.UtcNow);

        _senderMock
            .Setup(x => x.Send(It.Is<ListLessonsBySubjectIdQuery>(q => q.SubjectId == subjectId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(new List<PagedLessonBySubjectIdResponse> { lesson }));

        _projectionMock
            .Setup(x => x.FindAsync(
                It.IsAny<Expression<Func<Projection.LessonProgress, bool>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Projection.LessonProgress(Guid.NewGuid(), subjectId, lessonId, studentId, LessonStatus.Finished, DateTimeOffset.UtcNow));

        var query = new ListLessonProgressBySubjectIdQuery(subjectId, studentId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(1);
        result.Value.First().Subject.Should().BeEquivalentTo(lesson);
        result.Value.First().Status.Should().Be(LessonStatus.Finished);
    }

    [Fact]
    public async Task Handle_Should_Return_LessonProgress_With_Default_Pending_Status()
    {
        // Arrange
        var studentId = Guid.NewGuid();
        var subjectId = Guid.NewGuid();
        var lessonId = Guid.NewGuid();

        var lesson = new PagedLessonBySubjectIdResponse(
            lessonId, subjectId, "Título", "Conteúdo", 5, 1, DateTimeOffset.UtcNow);

        _senderMock
            .Setup(x => x.Send(It.IsAny<ListLessonsBySubjectIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(new List<PagedLessonBySubjectIdResponse> { lesson }));

        _projectionMock
            .Setup(x => x.FindAsync(It.IsAny<Expression<Func<Projection.LessonProgress, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Projection.LessonProgress?)null);

        var query = new ListLessonProgressBySubjectIdQuery(subjectId, studentId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().ContainSingle();
        result.Value.First().Status.Should().Be(LessonStatus.Pending);
    }

    [Fact]
    public async Task Handle_Should_Return_Failure_When_LessonsQuery_Fails()
    {
        // Arrange
        var studentId = Guid.NewGuid();
        var subjectId = Guid.NewGuid();

        var error = new Core.Shared.Errors.Error("Subject.NotFound", "Assunto não encontrado");

        _senderMock
            .Setup(x => x.Send(It.IsAny<ListLessonsBySubjectIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure<List<PagedLessonBySubjectIdResponse>>(error));

        var query = new ListLessonProgressBySubjectIdQuery(subjectId, studentId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(error);
    }
}
