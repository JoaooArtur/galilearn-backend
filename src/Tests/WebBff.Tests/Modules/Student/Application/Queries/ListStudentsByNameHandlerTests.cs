using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Core.Shared.Results;
using Moq;
using Student.Application.UseCases.Queries;
using Student.Domain;
using Student.Persistence.Projections;
using Student.Shared.Queries;
using Student.Shared.Response;
using Xunit;

namespace WebBff.Tests.Modules.Student.Application.Queries;
public class ListStudentsByNameHandlerTests
{
    private readonly Mock<IStudentProjection<Projection.Student>> _projectionMock;
    private readonly ListStudentsByNameHandler _handler;

    public ListStudentsByNameHandlerTests()
    {
        _projectionMock = new Mock<IStudentProjection<Projection.Student>>();
        _handler = new ListStudentsByNameHandler(_projectionMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Return_Students_Matching_Name()
    {
        // Arrange
        var query = new ListStudentsByNameQuery("ana");

        var students = new List<Projection.Student>
        {
            new(Guid.NewGuid(), "Ana Clara", "11999999999", "ana@email.com", "active", 1, 0, 50, 0, [], DateTimeOffset.Now.AddYears(-20), null, DateTimeOffset.Now),
            new(Guid.NewGuid(), "Joana Silva", "11999999998", "joana@email.com", "active", 1, 0, 50, 0, [], DateTimeOffset.Now.AddYears(-22), null, DateTimeOffset.Now),
            new(Guid.NewGuid(), "Carlos Souza", "11999999997", "carlos@email.com", "active", 1, 0, 50, 0, [], DateTimeOffset.Now.AddYears(-23), null, DateTimeOffset.Now)
        };

        _projectionMock
            .Setup(x => x.ListAsync(It.IsAny<Expression<Func<Projection.Student, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(students.Where(s => s.Name.ToLower().Contains("ana")).ToList());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Value.Count);
        Assert.All(result.Value, student => Assert.Contains("ana", student.Name.ToLower()));
    }
}
