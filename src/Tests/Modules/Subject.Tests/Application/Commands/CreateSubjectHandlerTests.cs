using System;
using System.Threading;
using System.Threading.Tasks;
using Core.Shared.Results;
using FluentAssertions;
using Moq;
using Serilog;
using Subject.Application.Services;
using Subject.Application.UseCases.Commands;
using Subject.Shared.Commands;
using SubjectAggregate = Subject.Domain.Aggregates.Subject;
using Subject.Shared.Response;
using Xunit;
using Subject.Domain.Aggregates;

namespace Student.Tests.Subject.Commands
{
    public class CreateSubjectHandlerTests
    {
        private readonly Mock<ISubjectApplicationService> _subjectServiceMock;
        private readonly Mock<ILogger> _loggerMock;
        private readonly CreateSubjectHandler _handler;

        public CreateSubjectHandlerTests()
        {
            _subjectServiceMock = new Mock<ISubjectApplicationService>();
            _loggerMock = new Mock<ILogger>();
            _handler = new CreateSubjectHandler(_subjectServiceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenSubjectIsCreated()
        {
            // Arrange
            var expectedId = Guid.NewGuid();
            var command = new CreateSubjectCommand("Português", "Língua nativa", 1);

            // Força criação com ID previsível se desejar (supondo que Subject.Create gere internamente)
            var subject = SubjectAggregate.Create(command.Name, command.Description, command.Index);

            var subjectIdField = typeof(SubjectAggregate)
                .GetField("_id", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            subjectIdField?.SetValue(subject, expectedId); // apenas se precisar validar ID fixo

            _subjectServiceMock
                .Setup(s => s.AppendEventsAsync(It.IsAny<SubjectAggregate>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Id.Should().NotBeEmpty(); // ou .Be(expectedId) se controlado
        }

        [Fact]
        public async Task Handle_ShouldPropagateException_WhenAppendFails()
        {
            // Arrange
            var command = new CreateSubjectCommand("História", "Conteúdo de história", 2);

            _subjectServiceMock
                .Setup(s => s.AppendEventsAsync(It.IsAny<SubjectAggregate>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Falha ao persistir"));

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Falha ao persistir");
        }
    }
}
