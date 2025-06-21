using Core.Application.Pagination;
using Core.Shared.Results;
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
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace WebBff.Tests.Modules.Subject.Application.Queries
{
    public class PagedLessonsBySubjectIdHandlerTests
    {
        private readonly Mock<ISubjectProjection<Projection.Lesson>> _lessonProjectionMock;
        private readonly Mock<ISubjectProjection<Projection.Question>> _questionProjectionMock;
        private readonly PagedLessonsBySubjectIdHandler _handler;

        public PagedLessonsBySubjectIdHandlerTests()
        {
            _lessonProjectionMock = new Mock<ISubjectProjection<Projection.Lesson>>();
            _questionProjectionMock = new Mock<ISubjectProjection<Projection.Question>>();
            _handler = new PagedLessonsBySubjectIdHandler(_lessonProjectionMock.Object, _questionProjectionMock.Object);
        }

        [Fact]
        public async Task Handle_Should_ReturnPagedLessons_WhenLessonsExist()
        {
            // Arrange
            var subjectId = Guid.NewGuid();
            var lessonId1 = Guid.NewGuid();
            var lessonId2 = Guid.NewGuid();
            var query = new PagedLessonsBySubjectIdQuery(subjectId, new Core.Domain.Primitives.Paging());

            var lessons = new List<Projection.Lesson>
            {
                new(lessonId1, subjectId, "Lesson 1", "Content 1", 0, DateTimeOffset.UtcNow),
                new(lessonId2, subjectId, "Lesson 2", "Content 2", 1, DateTimeOffset.UtcNow)
            };

                    var questions = new List<Projection.Question>
            {
                new Projection.Question(
                    Guid.NewGuid(),
                    lessonId1,
                    $"Question 1?",
                    QuestionLevel.Easy,
                    Enumerable.Range(1, 4).Select(j => new Dto.AnswerOption(Guid.NewGuid(), $"Option {j}")).ToList(),
                    DateTimeOffset.UtcNow
                ),
                new Projection.Question(
                    Guid.NewGuid(),
                    lessonId2,
                    $"Question 2?",
                    QuestionLevel.Easy,
                    Enumerable.Range(1, 4).Select(j => new Dto.AnswerOption(Guid.NewGuid(), $"Option {j}")).ToList(),
                    DateTimeOffset.UtcNow
                )
            };

            _lessonProjectionMock
                .Setup(x => x.ListAsync(It.IsAny<Expression<Func<Projection.Lesson, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(lessons);

            _questionProjectionMock
                .Setup(x => x.ListAsync(It.IsAny<Expression<Func<Projection.Question, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Expression<Func<Projection.Question, bool>> filter, CancellationToken _) =>
                {
                    var compiled = filter.Compile();
                    return questions.Where(compiled).ToList();
                });

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            var pagedResult = result.Value;
            Assert.Equal(2, pagedResult.Items.Count());
        }

        [Fact]
        public async Task Handle_Should_ReturnEmpty_WhenNoLessonsFound()
        {
            // Arrange
            var subjectId = Guid.NewGuid();
            var query = new PagedLessonsBySubjectIdQuery(subjectId, new Core.Domain.Primitives.Paging(1, 10));

            _lessonProjectionMock
                .Setup(x => x.ListAsync(l => l.SubjectId == subjectId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Projection.Lesson>());

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Empty(result.Value.Items);
        }
    }
}
