using Core.Shared.Results;
using FluentAssertions;
using Moq;
using Subject.Application.UseCases.Queries;
using Subject.Domain;
using Subject.Persistence.Projections;
using Subject.Shared.Queries;
using Subject.Shared.Response;
using System.Linq.Expressions;
using Xunit;

namespace Subject.Tests.Application.Queries;

public class GetLessonCountBySubjectIdHandlerTests
{
    private readonly Mock<ISubjectProjection<Projection.Lesson>> _projectionMock = new();
    private readonly GetLessonCountBySubjectIdHandler _handler;

    public GetLessonCountBySubjectIdHandlerTests()
    {
        _handler = new GetLessonCountBySubjectIdHandler(_projectionMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Return_Correct_Count_When_Lessons_Exist()
    {
        // Arrange
        var subjectId = Guid.NewGuid();
        var lessons = new List<Projection.Lesson>
        {
            new(Guid.NewGuid(), subjectId, "Title1", "Content1", 0, DateTimeOffset.UtcNow),
            new(Guid.NewGuid(), subjectId, "Title2", "Content2", 1, DateTimeOffset.UtcNow)
        };

        _projectionMock.Setup(x => x.ListAsync(
            It.IsAny<Expression<Func<Projection.Lesson, bool>>>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(lessons);

        var query = new GetLessonCountBySubjectIdQuery(subjectId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Count.Should().Be(2);
    }

    [Fact]
    public async Task Handle_Should_Return_Zero_When_Lessons_Is_Null()
    {
        // Arrange
        var subjectId = Guid.NewGuid();

        _projectionMock.Setup(x => x.ListAsync(
            It.IsAny<Expression<Func<Projection.Lesson, bool>>>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync((List<Projection.Lesson>?)null);

        var query = new GetLessonCountBySubjectIdQuery(subjectId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Count.Should().Be(0);
    }
}
