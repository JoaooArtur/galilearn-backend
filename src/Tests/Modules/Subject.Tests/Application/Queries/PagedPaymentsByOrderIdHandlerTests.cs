using Core.Application.Pagination;
using Core.Domain.Primitives;
using Core.Shared.Results;
using Moq;
using Subject.Application.UseCases.Queries;
using Subject.Domain;
using Subject.Persistence.Projections;
using Subject.Shared.Queries;
using Subject.Shared.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Subject.Tests.Application.Queries
{
    public class PagedPaymentsByOrderIdHandlerTests
    {
        private readonly Mock<ISubjectProjection<Projection.Subject>> _projectionMock;
        private readonly PagedPaymentsByOrderIdHandler _handler;

        public PagedPaymentsByOrderIdHandlerTests()
        {
            _projectionMock = new();
            _handler = new PagedPaymentsByOrderIdHandler(_projectionMock.Object);
        }

        [Fact]
        public async Task Handle_Should_ReturnPagedSubjects_WhenSubjectsExist()
        {
            // Arrange
            var paging = new Paging();
            var query = new PagedSubjectQuery(paging);

            var subjects = new List<Projection.Subject>
            {
                new(Guid.NewGuid(), "Math", "Basic Math", 0, DateTimeOffset.UtcNow),
                new(Guid.NewGuid(), "Science", "General Science", 1, DateTimeOffset.UtcNow)
            };

            _projectionMock
                .Setup(x => x.ListAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(subjects);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(2, result.Value.Items.Count());
        }

        [Fact]
        public async Task Handle_Should_ReturnEmptyPagedResult_WhenSubjectsIsNull()
        {
            // Arrange
            var query = new PagedSubjectQuery(new Paging(1, 10));

            _projectionMock
                .Setup(x => x.ListAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync((List<Projection.Subject>?)null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Null(result.Value);
        }
    }
}
