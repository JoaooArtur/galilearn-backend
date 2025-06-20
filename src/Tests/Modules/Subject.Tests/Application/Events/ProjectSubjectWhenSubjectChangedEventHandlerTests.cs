using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Serilog;
using Subject.Application.UseCases.Events;
using Subject.Domain;
using Subject.Persistence.Projections;
using Xunit;

namespace Subject.Tests.Application.Events
{
    public class ProjectSubjectWhenSubjectChangedEventHandlerTests
    {
        private readonly Mock<ISubjectProjection<Projection.Subject>> _projectionMock;
        private readonly Mock<ILogger> _loggerMock;
        private readonly ProjectSubjectWhenSubjectChangedEventHandler _handler;

        public ProjectSubjectWhenSubjectChangedEventHandlerTests()
        {
            _projectionMock = new Mock<ISubjectProjection<Projection.Subject>>();
            _loggerMock = new Mock<ILogger>();
            _handler = new ProjectSubjectWhenSubjectChangedEventHandler(_projectionMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Handle_SubjectCreated_ShouldInsertProjection()
        {
            // Arrange
            var @event = new DomainEvent.SubjectCreated(
                Guid.NewGuid(),
                "Matemática",
                "Conteúdo de álgebra",
                1,
                DateTimeOffset.UtcNow,
                1
            );

            _projectionMock
                .Setup(p => p.ReplaceInsertAsync(It.IsAny<Projection.Subject>(), It.IsAny<CancellationToken>()))
                .Returns(ValueTask.CompletedTask);

            // Act
            Func<Task> act = async () => await _handler.Handle(@event);

            // Assert
            await act.Should().NotThrowAsync();

            _projectionMock.Verify(p => p.ReplaceInsertAsync(
                It.Is<Projection.Subject>(s =>
                    s.Id == @event.SubjectId &&
                    s.Name == @event.Name &&
                    s.Description == @event.Description &&
                    s.Index == @event.Index &&
                    s.CreatedAt == @event.Timestamp),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_SubjectDeleted_ShouldDeleteProjection()
        {
            // Arrange
            var subjectId = Guid.NewGuid();
            var @event = new DomainEvent.SubjectDeleted(subjectId, 1);

            _projectionMock
                .Setup(p => p.DeleteAsync(It.IsAny<Expression<Func<Projection.Subject, bool>>>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            Func<Task> act = async () => await _handler.Handle(@event);

            // Assert
            await act.Should().NotThrowAsync();

            _projectionMock.Verify(p => p.DeleteAsync(
                It.Is<Expression<Func<Projection.Subject, bool>>>(expr =>
                    expr != null &&
                    expr.ToString().Contains("x.Id ==")),
                It.IsAny<CancellationToken>()),
            Times.Once);
        }

        [Fact]
        public async Task Handle_SubjectCreated_ShouldLogError_WhenExceptionThrown()
        {
            // Arrange
            var @event = new DomainEvent.SubjectCreated(
                Guid.NewGuid(), "Erro", "Falha", 1, DateTimeOffset.UtcNow, 1);

            _projectionMock
                .Setup(p => p.ReplaceInsertAsync(It.IsAny<Projection.Subject>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Erro simulado"));

            // Act
            Func<Task> act = async () => await _handler.Handle(@event);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Erro simulado");

            _loggerMock.Verify(l => l.Error(
                It.IsAny<Exception>(),
                $"Falha ao criar o assunto: {@event.SubjectId}."), Times.Once);
        }

        [Fact]
        public async Task Handle_SubjectDeleted_ShouldLogError_WhenExceptionThrown()
        {
            // Arrange
            var @event = new DomainEvent.SubjectDeleted(Guid.NewGuid(), 1);

            _projectionMock
                .Setup(p => p.DeleteAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Projection.Subject, bool>>>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Falha ao deletar"));

            // Act
            Func<Task> act = async () => await _handler.Handle(@event);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Falha ao deletar");

            _loggerMock.Verify(l => l.Error(
                It.IsAny<Exception>(),
                $"Falha ao deletar o assunto: {@event.SubjectId}."), Times.Once);
        }
    }
}
