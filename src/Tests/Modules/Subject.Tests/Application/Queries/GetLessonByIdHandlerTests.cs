using Core.Shared.Errors;
using Core.Shared.Results;
using FluentAssertions;
using Moq;
using Subject.Domain;
using Subject.Persistence.Projections;
using Subject.Shared.Queries;
using Subject.Shared.Response;
using Lesson.Application.UseCases.Queries;
using Xunit;

namespace Subject.Tests.Application.Queries;

public class GetLessonByIdHandlerTests
{
    private readonly Mock<ISubjectProjection<Projection.Lesson>> _projectionMock = new();
    private readonly GetLessonByIdHandler _handler;

    public GetLessonByIdHandlerTests()
    {
        _handler = new GetLessonByIdHandler(_projectionMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Return_Lesson_When_Found()
    {
        // Arrange
        var lessonId = Guid.NewGuid();
        var subjectId = Guid.NewGuid();
        var now = DateTimeOffset.UtcNow;

        var lesson = new Projection.Lesson(
            lessonId,
            subjectId,
            "Título da Lição",
            "Conteúdo da Lição",
            1,
            now);

        _projectionMock.Setup(x => x.GetAsync(lessonId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(lesson);

        var query = new GetLessonByIdQuery(lessonId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(new LessonResponse(
            lesson.Id,
            lesson.SubjectId,
            lesson.Title,
            lesson.Content,
            lesson.Index,
            lesson.CreatedAt));
    }

    [Fact]
    public async Task Handle_Should_Return_Failure_When_Lesson_Not_Found()
    {
        // Arrange
        var lessonId = Guid.NewGuid();

        _projectionMock.Setup(x => x.GetAsync(lessonId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Projection.Lesson?)null);

        var query = new GetLessonByIdQuery(lessonId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeOfType<NotFoundError>();
        result.Error.Code.Should().Be("LessonNotFound");
    }
}
