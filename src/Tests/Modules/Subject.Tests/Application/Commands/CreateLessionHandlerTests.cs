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

namespace Subject.Tests.Application.Commands
{
    public class CreateLessonHandlerTests
    {
        private readonly Mock<ISubjectApplicationService> _subjectServiceMock;
        private readonly Mock<ILogger> _loggerMock;
        private readonly CreateLessionHandler _handler;

        public CreateLessonHandlerTests()
        {
            _subjectServiceMock = new Mock<ISubjectApplicationService>();
            _loggerMock = new Mock<ILogger>();
            _handler = new CreateLessionHandler(_subjectServiceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenLessonIsCreated()
        {
            // Arrange
            var subjectId = Guid.NewGuid();
            var expectedLessonId = Guid.NewGuid();
            var command = new CreateLessonCommand(subjectId, "Título da Lição", "Conteúdo", 1);

            var subject = SubjectAggregate.Create("Matemática", "Básico", 1);

            // Força retorno previsível (caso o AddLesson gere ID internamente e você queira validar o ID exato)
            var lessonIdField = typeof(SubjectAggregate)
                .GetField("_lastGeneratedLessonId", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            lessonIdField?.SetValue(subject, expectedLessonId); // se campo existir

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
            result.Value.Id.Should().NotBeEmpty(); // ou .Be(expectedLessonId) se controle for garantido
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenSubjectNotFound()
        {
            // Arrange
            var command = new CreateLessonCommand(Guid.NewGuid(), "Qualquer", "Conteúdo", 1);

            _subjectServiceMock
                .Setup(s => s.LoadAggregateAsync<SubjectAggregate>(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Failure<SubjectAggregate>(new Core.Shared.Errors.Error("Erro", "Subject not found")));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.ToString().Should().Be("Erro");
        }
    }
}
