using System;
using System.Threading;
using System.Threading.Tasks;
using Core.Shared.Results;
using FluentAssertions;
using Moq;
using Serilog;
using Subject.Application.Services;
using Subject.Application.UseCases.Commands;
using Subject.Domain.Aggregates;
using Subject.Shared.Commands;
using SubjectAggregate = Subject.Domain.Aggregates.Subject;
using Subject.Shared.Response;
using Xunit;
using Subject.Domain.Enumerations;

namespace Subject.Tests.Application.Commands
{
    public class CreateQuestionHandlerTests
    {
        private readonly Mock<ISubjectApplicationService> _subjectServiceMock;
        private readonly Mock<ILogger> _loggerMock;
        private readonly CreateQuestionHandler _handler;

        public CreateQuestionHandlerTests()
        {
            _subjectServiceMock = new Mock<ISubjectApplicationService>();
            _loggerMock = new Mock<ILogger>();
            _handler = new CreateQuestionHandler(_subjectServiceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenQuestionIsCreated()
        {
            // Arrange
            var subjectId = Guid.NewGuid();
            var lessonId = Guid.NewGuid();
            var expectedQuestionId = Guid.NewGuid();

            var command = new CreateQuestionCommand(subjectId, lessonId, "Qual a capital do Brasil?", QuestionLevel.Easy);

            var subject = SubjectAggregate.Create("Geografia", "Iniciante", 1);

            // (Opcional) Forçar ID se você quiser testar o valor específico retornado
            var questionIdField = typeof(SubjectAggregate)
                .GetField("_lastGeneratedQuestionId", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            questionIdField?.SetValue(subject, expectedQuestionId); // caso haja controle por campo

            _subjectServiceMock
                .Setup(s => s.LoadAggregateAsync<SubjectAggregate>(subjectId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Success(subject));

            _subjectServiceMock
                .Setup(s => s.AppendEventsAsync(subject, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Id.Should().NotBeEmpty(); // ou .Be(expectedQuestionId) se o ID for previsível
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenSubjectNotFound()
        {
            // Arrange
            var command = new CreateQuestionCommand(Guid.NewGuid(), Guid.NewGuid(), "Texto qualquer", QuestionLevel.Medium);

            _subjectServiceMock
                .Setup(s => s.LoadAggregateAsync<SubjectAggregate>(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Failure<SubjectAggregate>(new Core.Shared.Errors.Error("Erro", "Aggregate not Found")));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Message.ToString().Should().Be("Aggregate not Found");
        }
    }
}
