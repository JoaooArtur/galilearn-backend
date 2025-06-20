using FluentAssertions;
using Moq;
using Subject.Application.UseCases.Queries;
using Subject.Domain;
using Subject.Persistence.Projections;
using Subject.Shared.Queries;
using Subject.Shared.Response;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Subject.Tests.Application.Queries
{
    public class GetSubjectByIdHandlerTests
    {
        private readonly Mock<ISubjectProjection<Projection.Subject>> _projectionMock;
        private readonly GetSubjectByIdHandler _handler;

        public GetSubjectByIdHandlerTests()
        {
            _projectionMock = new Mock<ISubjectProjection<Projection.Subject>>();
            _handler = new GetSubjectByIdHandler(_projectionMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Return_SubjectResponse_When_Found()
        {
            // Arrange
            var subjectId = Guid.NewGuid();

            var subject = new Projection.Subject(
                Id: subjectId,
                Name: "Matemática",
                Description: "Descrição de Matemática",
                Index: 1,
                CreatedAt: DateTimeOffset.UtcNow
            );

            _projectionMock
                .Setup(x => x.GetAsync(subjectId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(subject);

            var query = new GetSubjectByIdQuery(subjectId);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Id.Should().Be(subjectId);
            result.Value.Name.Should().Be("Matemática");
            result.Value.Description.Should().Be("Descrição de Matemática");
        }

        [Fact]
        public async Task Handle_Should_Return_Failure_When_Subject_NotFound()
        {
            // Arrange
            var subjectId = Guid.NewGuid();

            _projectionMock
                .Setup(x => x.GetAsync(subjectId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Projection.Subject?)null);

            var query = new GetSubjectByIdQuery(subjectId);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Code.Should().Be("SubjectNotFound");
        }
    }
}
