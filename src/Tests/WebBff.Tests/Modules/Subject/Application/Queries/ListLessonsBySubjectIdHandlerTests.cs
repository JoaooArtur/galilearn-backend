using FluentAssertions;
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

namespace WebBff.Tests.Modules.Subject.Application.Queries
{
    public class ListLessonsBySubjectIdHandlerTests
    {
        private readonly Mock<ISubjectProjection<Projection.Lesson>> _lessonProjectionMock;
        private readonly Mock<ISubjectProjection<Projection.Question>> _questionProjectionMock;
        private readonly ListLessonsBySubjectIdHandler _handler;

        public ListLessonsBySubjectIdHandlerTests()
        {
            _lessonProjectionMock = new Mock<ISubjectProjection<Projection.Lesson>>();
            _questionProjectionMock = new Mock<ISubjectProjection<Projection.Question>>();
            _handler = new ListLessonsBySubjectIdHandler(_lessonProjectionMock.Object, _questionProjectionMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Return_LessonResponses_With_QuestionCount()
        {
            // Arrange
            var subjectId = Guid.NewGuid();
            var lessonId = Guid.NewGuid();
            var createdAt = DateTimeOffset.UtcNow;

            var lessons = new List<Projection.Lesson>
            {
                new Projection.Lesson(lessonId, subjectId, "Lesson 1", "Content 1", 1, createdAt)
            };

            var questions = new List<Projection.Question>
            {
                new Projection.Question(Guid.NewGuid(), lessonId, "Pergunta 1", QuestionLevel.Easy, [], createdAt)
            };

            _lessonProjectionMock
                .Setup(x => x.ListAsync(It.IsAny<Expression<Func<Projection.Lesson, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(lessons);

            _questionProjectionMock
                .Setup(x => x.ListAsync(It.IsAny<Expression<Func<Projection.Question, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(questions);

            var query = new ListLessonsBySubjectIdQuery(subjectId);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().HaveCount(1);
            result.Value.First().QuestionsCount.Should().Be(1);
            result.Value.First().Title.Should().Be("Lesson 1");
        }

        [Fact]
        public async Task Handle_Should_Return_EmptyList_When_NoLessons()
        {
            // Arrange
            var subjectId = Guid.NewGuid();
            _lessonProjectionMock
                .Setup(x => x.ListAsync(It.IsAny<Expression<Func<Projection.Lesson, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Projection.Lesson>());

            var query = new ListLessonsBySubjectIdQuery(subjectId);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeEmpty();
        }
    }
}
