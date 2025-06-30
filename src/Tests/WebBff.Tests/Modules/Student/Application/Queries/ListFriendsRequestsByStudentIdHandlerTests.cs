using Core.Shared.Results;
using FluentAssertions;
using Moq;
using Student.Application.UseCases.Queries;
using Student.Domain.Enumerations;
using Student.Persistence.Projections;
using Student.Shared.Queries;
using Student.Shared.Response;
using Student.Domain;
using Xunit;
using MediatR;
using System.Linq.Expressions;

namespace WebBff.Tests.Modules.Student.Application.Queries;

public class ListFriendsRequestsByStudentIdHandlerTests
{
    private readonly Mock<IStudentProjection<Projection.FriendRequests>> _projectionMock = new();
    private readonly Mock<IStudentProjection<Projection.Student>> _studentProjectionMock = new();
    private readonly ListFriendsRequestsByStudentIdHandler _handler;

    public ListFriendsRequestsByStudentIdHandlerTests()
    {
        _handler = new ListFriendsRequestsByStudentIdHandler(_projectionMock.Object, _studentProjectionMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Return_List_Of_Requests_When_Exists()
    {
        // Arrange
        var studentId = Guid.NewGuid();
        var friendId = Guid.NewGuid();
        var requestId = Guid.NewGuid();
        var now = DateTimeOffset.UtcNow;

        var requests = new List<Projection.FriendRequests>
        {
            new Projection.FriendRequests(requestId, studentId, friendId, FriendRequestStatus.Pending, now)
        };

        var student = new Projection.Student(
            Guid.NewGuid(),
            "Aluno",
            "+5511111111",
            "email@test.com",
            StudentStatus.Active,
            2,
            0,
            200,
            1,
            [],
            DateTime.Now,
            null,
            DateTime.Now);

        _projectionMock
            .Setup(x => x.ListAsync(It.IsAny<Expression<Func<Projection.FriendRequests, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(requests);
        _studentProjectionMock
            .Setup(x => x.FindAsync(It.IsAny<Expression<Func<Projection.Student, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(student);

        var query = new ListFriendsRequestsByStudentIdQuery(studentId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(1);
        result.Value.First().Should().BeEquivalentTo(new ListFriendsRequestsByStudentIdResponse(
            requestId,
            studentId,
            friendId,
            "Aluno",
            2,
            FriendRequestStatus.Pending,
            now));
    }

    [Fact]
    public async Task Handle_Should_Return_Default_When_NoRequestsFound()
    {
        // Arrange
        var studentId = Guid.NewGuid();

        _projectionMock
            .Setup(x => x.ListAsync(It.IsAny<Expression<Func<Projection.FriendRequests, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((List<Projection.FriendRequests>)null!); // simulate null

        var query = new ListFriendsRequestsByStudentIdQuery(studentId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEmpty();
    }
}
