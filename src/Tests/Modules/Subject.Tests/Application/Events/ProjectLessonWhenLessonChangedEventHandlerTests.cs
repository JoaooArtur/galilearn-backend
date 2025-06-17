using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Serilog;
using Subject.Application.UseCases.Events;
using Subject.Domain;
using Subject.Persistence.Projections;
using Xunit;

namespace Student.Tests.Subject.Events
{
    public class ProjectLessonWhenLessonChangedEventHandlerTests
    {
        private readonly Mock<ISubjectProjection<Projection.Lesson>> _projectionMock;
        private readonly Mock<ILogger> _loggerMock;
        private readonly ProjectLessonWhenLessonChangedEventHandler _handler;

        public ProjectLessonWhenLessonChangedEventHandlerTests()
        {
            _projectionMock = new Mock<ISubjectProjection<Projection.Lesson>>();
            _loggerMock = new Mock<ILogger>();
            _handler = new ProjectLessonWhenLessonChangedEventHandler(_projectionMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldInsertProjection_WhenEventIsValid()
        {
            // Arrange
            var lessonAdded = new DomainEvent.LessonAdded(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Título da aula",
                "Conteúdo",
                1,
                DateTimeOffset.UtcNow,
                1
            );

            _projectionMock
                .Setup(p => p.ReplaceInsertAsync(It.IsAny<Projection.Lesson>(), It.IsAny<CancellationToken>()))
                .Returns(ValueTask.CompletedTask);

            // Act
            Func<Task> act = async () => await _handler.Handle(lessonAdded);

            // Assert
            await act.Should().NotThrowAsync();
            _projectionMock.Verify(p => p.ReplaceInsertAsync(
                It.Is<Projection.Lesson>(l =>
                    l.Id == lessonAdded.LessonId &&
                    l.SubjectId == lessonAdded.SubjectId &&
                    l.Title == lessonAdded.Title &&
                    l.Content == lessonAdded.Content &&
                    l.Index == lessonAdded.Index &&
                    l.CreatedAt == lessonAdded.Timestamp),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldLogError_AndRethrow_WhenExceptionIsThrown()
        {
            // Arrange
            var lessonAdded = new DomainEvent.LessonAdded(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Falha ao salvar",
                "Erro",
                1,
                DateTimeOffset.UtcNow,
                1
            );

            _projectionMock
                .Setup(p => p.ReplaceInsertAsync(It.IsAny<Projection.Lesson>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Erro simulado"));

            // Act
            Func<Task> act = async () => await _handler.Handle(lessonAdded);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Erro simulado");

            _loggerMock.Verify(l => l.Error(It.IsAny<Exception>(), $"Falha ao criar a aula: {lessonAdded.LessonId}."), Times.Once);
        }
    }
}
