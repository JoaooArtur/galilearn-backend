using FluentAssertions;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Subject.Application.UseCases.Queries;
using Subject.Domain;
using Subject.Domain.Enumerations;
using Subject.Persistence.Projections;
using Subject.Shared.Queries;
using Subject.Shared.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Subject.Tests.Application.Queries
{
    public class GetRandomQuestionsByLessionIdHandlerTests
    {
        private readonly Mock<ISubjectProjection<Projection.Question>> _projectionMock = new();
        private readonly GetRandomQuestionsByLessionIdHandler _handler;

        public GetRandomQuestionsByLessionIdHandlerTests()
        {
            _handler = new GetRandomQuestionsByLessionIdHandler(_projectionMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Return_5_Shuffled_Questions_When_Exists()
        {
            // Arrange
            var lessonId = Guid.NewGuid();
            var questions = Enumerable.Range(1, 10).Select(i =>
                new Projection.Question(
                    Guid.NewGuid(),
                    lessonId,
                    $"Question {i}?",
                    QuestionLevel.Easy,
                    [new Dto.AnswerOption(Guid.NewGuid(), $"Option {i}")],
                    DateTimeOffset.UtcNow
                )).ToList();

            _projectionMock
                .Setup(x => x.ListAsync(It.IsAny<Expression<Func<Projection.Question, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(questions);

            var query = new ListRandomQuestionsByLessonIdQuery(lessonId);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().HaveCount(5);
            result.Value.Should().OnlyHaveUniqueItems();
        }

        [Fact]
        public async Task Handle_Should_Return_Failure_When_Questions_NotFound()
        {
            // Arrange
            var lessonId = Guid.NewGuid();

            _projectionMock
                .Setup(x => x.ListAsync(q => q.LessonId == lessonId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((List<Projection.Question>?)null);

            var query = new ListRandomQuestionsByLessonIdQuery(lessonId);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Code.Should().Be("QuestionsNotFound");
        }
    }
}
