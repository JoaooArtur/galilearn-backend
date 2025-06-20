using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Shared.Results;
using FluentAssertions;
using Moq;
using Serilog;
using Subject.Application.UseCases.Commands;
using Subject.Persistence.Projections;
using Subject.Shared.Commands;
using Subject.Domain;
using Subject.Shared.Response;
using Xunit;

namespace Subject.Tests.Application.Commands
{
    public class CheckCorrectAnswerHandlerTests
    {
        private readonly Mock<ISubjectProjection<Projection.Question>> _projectionMock;
        private readonly Mock<ILogger> _loggerMock;
        private readonly CheckCorrectAnswerHandler _handler;

        public CheckCorrectAnswerHandlerTests()
        {
            _projectionMock = new Mock<ISubjectProjection<Projection.Question>>();
            _loggerMock = new Mock<ILogger>();
            _handler = new CheckCorrectAnswerHandler(_projectionMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnCorrect_WhenAnswerIsCorrect()
        {
            // Arrange
            var questionId = Guid.NewGuid();
            var correctAnswerId = Guid.NewGuid();

            var question = new Projection.Question(
                Id: questionId,
                LessonId: Guid.NewGuid(),
                Text: "Qual a capital da França?",
                Level: "Fácil",
                Answers: new List<Dto.AnswerOption>
                {
                    new(correctAnswerId, "Paris", true),
                    new(Guid.NewGuid(), "Londres", false)
                },
                CreatedAt: DateTimeOffset.UtcNow
            );

            _projectionMock.Setup(p => p.GetAsync(questionId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(question);

            var command = new CheckCorrectAnswerCommand(questionId, correctAnswerId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.correctAnswerId.Should().Be(correctAnswerId);
            result.Value.IsCorrectAnswer.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_ShouldReturnIncorrect_WhenAnswerIsWrong()
        {
            // Arrange
            var questionId = Guid.NewGuid();
            var correctAnswerId = Guid.NewGuid();
            var wrongAnswerId = Guid.NewGuid();

            var question = new Projection.Question(
                Id: questionId,
                LessonId: Guid.NewGuid(),
                Text: "Qual a capital da França?",
                Level: "Fácil",
                Answers: new List<Dto.AnswerOption>
                {
                    new(correctAnswerId, "Paris", true),
                    new(wrongAnswerId, "Berlim", false)
                },
                CreatedAt: DateTimeOffset.UtcNow
            );

            _projectionMock.Setup(p => p.GetAsync(questionId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(question);

            var command = new CheckCorrectAnswerCommand(questionId, wrongAnswerId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.correctAnswerId.Should().Be(correctAnswerId);
            result.Value.IsCorrectAnswer.Should().BeFalse();
        }
    }
}
