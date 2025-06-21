using Core.Shared.Results;
using Moq;
using Subject.Application.UseCases.Queries;
using Subject.Domain;
using Subject.Persistence.Projections;
using Subject.Shared.Queries;
using Subject.Shared.Response;
using Xunit;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace WebBff.Tests.Modules.Subject.Application.Queries
{
    public class ListSubjectsHandlerTests
    {
        private readonly Mock<ISubjectProjection<Projection.Subject>> _projectionMock;
        private readonly ListSubjectsHandler _handler;

        public ListSubjectsHandlerTests()
        {
            _projectionMock = new Mock<ISubjectProjection<Projection.Subject>>();
            _handler = new ListSubjectsHandler(_projectionMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Return_Subjects_When_Exists()
        {
            // Arrange
            var subjects = new List<Projection.Subject>
            {
                new(Guid.NewGuid(), "Matemática", "Descrição A", 1, DateTimeOffset.UtcNow),
                new(Guid.NewGuid(), "História", "Descrição B", 2, DateTimeOffset.UtcNow)
            };

            _projectionMock
                .Setup(x => x.ListAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(subjects);

            var query = new ListSubjectsQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(2, result.Value.Count);
            Assert.Contains(result.Value, x => x.Name == "Matemática");
        }

        [Fact]
        public async Task Handle_Should_Return_Empty_When_Subjects_Null()
        {
            // Arrange
            _projectionMock
                .Setup(x => x.ListAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync((List<Projection.Subject>)null);

            var query = new ListSubjectsQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Null(result.Value);
        }
    }
}
