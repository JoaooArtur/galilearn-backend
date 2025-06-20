using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using MongoDB.Driver;
using Serilog;
using Subject.Application.UseCases.Events;
using Subject.Domain;
using Subject.Persistence.Projections;
using Xunit;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Subject.Domain.Enumerations;

namespace Subject.Tests.Application.Events
{
    public class ProjectQuestionWhenQuestionChangedEventHandlerTests
    {
        private readonly Mock<ISubjectProjection<Projection.Question>> _projectionMock;
        private readonly Mock<ILogger> _loggerMock;
        private readonly Mock<IMongoCollection<Projection.Question>> _collectionMock;
        private readonly ProjectQuestionWhenQuestionChangedEventHandler _handler;

        public ProjectQuestionWhenQuestionChangedEventHandlerTests()
        {
            _projectionMock = new Mock<ISubjectProjection<Projection.Question>>();
            _loggerMock = new Mock<ILogger>();
            _collectionMock = new Mock<IMongoCollection<Projection.Question>>();
            _handler = new ProjectQuestionWhenQuestionChangedEventHandler(_projectionMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Handle_QuestionAdded_ShouldInsertProjection()
        {
            // Arrange
            var @event = new DomainEvent.QuestionAdded(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Qual a capital do Brasil?",
                QuestionLevel.Easy,
                DateTimeOffset.UtcNow,
                1
            );

            _projectionMock
                .Setup(p => p.ReplaceInsertAsync(It.IsAny<Projection.Question>(), It.IsAny<CancellationToken>()))
                .Returns(ValueTask.CompletedTask);

            // Act
            Func<Task> act = async () => await _handler.Handle(@event);

            // Assert
            await act.Should().NotThrowAsync();

            _projectionMock.Verify(p => p.ReplaceInsertAsync(
                It.Is<Projection.Question>(q =>
                    q.Id == @event.QuestionId &&
                    q.LessonId == @event.LessonId &&
                    q.Text == @event.Text &&
                    q.Level == @event.Level &&
                    q.CreatedAt == @event.Timestamp &&
                    q.Answers.Count == 0),
                It.IsAny<CancellationToken>()), Times.Once);
        }
        [Fact]
        public async Task Handle_AnswerOptionAdded_ShouldUpdateProjection()
        {
            // Arrange
            var questionId = Guid.NewGuid();
            var lessonId = Guid.NewGuid();
            var answerOptionId = Guid.NewGuid();
            var answerText = "Brasília";

            var @event = new DomainEvent.AnswerOptionAdded(
                lessonId,
                questionId,
                answerOptionId,
                answerText,
                true,
                DateTime.Now,
                1
            );

            var question = new Projection.Question(
                Id: questionId,
                LessonId: Guid.NewGuid(),
                Text: "Qual a capital do Brasil?",
                Level: "Fácil",
                Answers: new List<Dto.AnswerOption>(),
                CreatedAt: DateTimeOffset.UtcNow
            );

            _projectionMock.Setup(p => p.GetCollection())
                .Returns(_collectionMock.Object);

            _projectionMock.Setup(p => p.GetAsync(questionId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(question);

            _collectionMock
                .Setup(c => c.UpdateOneAsync(
                    It.IsAny<FilterDefinition<Projection.Question>>(),
                    It.IsAny<UpdateDefinition<Projection.Question>>(),
                    It.IsAny<UpdateOptions>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(Mock.Of<UpdateResult>())
                .Verifiable();

            // Act
            Func<Task> act = async () => await _handler.Handle(@event);

            // Assert
            await act.Should().NotThrowAsync();

            _collectionMock.Verify(c => c.UpdateOneAsync(
                It.Is<FilterDefinition<Projection.Question>>(f => f != null),
                It.Is<UpdateDefinition<Projection.Question>>(u => u != null),
                It.Is<UpdateOptions>(opts => opts.IsUpsert == true),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_AnswerOptionAdded_ShouldLogError_WhenExceptionIsThrown()
        {
            // Arrange
            var @event = new DomainEvent.AnswerOptionAdded(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Erro",
                false,
                DateTimeOffset.Now,
                1
            );

            _projectionMock.Setup(p => p.GetCollection())
                .Throws(new Exception("Erro simulado"));

            // Act
            Func<Task> act = async () => await _handler.Handle(@event);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Erro simulado");

            _loggerMock.Verify(l => l.Error(
                It.IsAny<Exception>(),
                $"Falha ao adicionar a resposta:{@event.AnswerOptionId} para a questão: {@event.QuestionId}."), Times.Once);
        }
    }
}
