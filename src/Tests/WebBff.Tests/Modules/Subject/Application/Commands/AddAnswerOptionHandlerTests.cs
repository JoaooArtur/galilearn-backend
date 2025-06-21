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

namespace WebBff.Tests.Modules.Subject.Application.Commands
{
    public class AddAnswerOptionHandlerTests
    {
        private readonly Mock<ISubjectApplicationService> _subjectServiceMock;
        private readonly Mock<ILogger> _loggerMock;
        private readonly AddAnswerOptionHandler _handler;

        public AddAnswerOptionHandlerTests()
        {
            _subjectServiceMock = new Mock<ISubjectApplicationService>();
            _loggerMock = new Mock<ILogger>();
            _handler = new AddAnswerOptionHandler(_subjectServiceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenAnswerIsAdded()
        {
            // Arrange
            var subjectId = Guid.NewGuid();
            var lessonId = Guid.NewGuid();
            var questionId = Guid.NewGuid();
            var expectedAnswerId = Guid.NewGuid();

            var subject = SubjectAggregate.Create("Matemática", "Básico", 1);

            // Simula estado interno se necessário, ou reflita uma abordagem real de como o AddQuestionAnswerOption se comportaria
            // Suponha que internamente ele retorna o Guid abaixo, como se fosse gerado pelo domínio:
            var privateField = typeof(SubjectAggregate)
                .GetField("_answers", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Simula o retorno controlado (alternativa: usar interface/factory para injetar o Guid)
            var answerIdField = typeof(SubjectAggregate)
                .GetField("_lastGeneratedAnswerId", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            answerIdField?.SetValue(subject, expectedAnswerId); // se houver esse campo

            _subjectServiceMock
                .Setup(s => s.LoadAggregateAsync<SubjectAggregate>(subjectId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Success(subject));

            _subjectServiceMock
                .Setup(s => s.AppendEventsAsync(subject, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var command = new AddAnswerOptionCommand(subjectId, lessonId, questionId, "Texto", true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Id.Should().NotBeEmpty(); // ou .Be(expectedAnswerId) se for controlado
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenSubjectNotFound()
        {
            // Arrange
            var command = new AddAnswerOptionCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "Texto", true);

            _subjectServiceMock
                .Setup(s => s.LoadAggregateAsync<SubjectAggregate>(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Failure<SubjectAggregate>(new Core.Shared.Errors.Error("Erro","Subject not found")));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Message.Should().Be("Subject not found");
        }
    }
}