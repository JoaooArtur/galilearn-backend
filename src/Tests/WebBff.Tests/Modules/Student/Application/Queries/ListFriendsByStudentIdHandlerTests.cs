using Core.Shared.Results;
using FluentAssertions;
using Moq;
using Student.Application.UseCases.Queries;
using Student.Persistence.Projections;
using Student.Shared.Queries;
using Student.Shared.Response;
using Student.Domain;
using Xunit;
using MediatR;

namespace WebBff.Tests.Modules.Student.Application.Queries;

public class ListFriendsByStudentIdHandlerTests
{
    private readonly Mock<IStudentProjection<Projection.Student>> _projectionMock = new();
    private readonly Mock<ISender> _senderMock = new();
    private readonly ListFriendsByStudentIdHandler _handler;

    public ListFriendsByStudentIdHandlerTests()
    {
        _handler = new ListFriendsByStudentIdHandler(_projectionMock.Object, _senderMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Return_Ordered_Friends_List()
    {
        // Arrange
        var studentId = Guid.NewGuid();
        var friendId1 = Guid.NewGuid();
        var friendId2 = Guid.NewGuid();

        var student = new Projection.Student(
            Id: studentId,
            Name: "Maria",
            Phone: "+5511988888888",
            Email: "teste@email.com",
            Status: "Active",
            Level: 1,
            Xp: 0,
            NextLevelXPNeeded: 100,
            DaysStreak: 0,
            Friends: [ friendId1, friendId2],
            DateOfBirth: DateTimeOffset.Parse("1999-05-05"),
            LastLessonAnswered: null,
            CreatedAt: DateTimeOffset.UtcNow
        );

        var friend1 = new Projection.Student(
            Id: friendId1,
            Name: "Augusto",
            Phone: "+5511988888888",
            Email: "teste@email.com",
            Status: "Active",
            Level: 5,
            Xp: 0,
            NextLevelXPNeeded: 100,
            DaysStreak: 3,
            Friends: [],
            DateOfBirth: DateTimeOffset.Parse("1999-05-05"),
            LastLessonAnswered: null,
            CreatedAt: DateTimeOffset.UtcNow
        );

        var friend2 = new Projection.Student(
            Id: friendId2,
            Name: "Renan",
            Phone: "+5511988888888",
            Email: "teste@email.com",
            Status: "Active",
            Level: 5,
            Xp: 0,
            NextLevelXPNeeded: 100,
            DaysStreak: 1,
            Friends: [],
            DateOfBirth: DateTimeOffset.Parse("1999-05-05"),
            LastLessonAnswered: null,
            CreatedAt: DateTimeOffset.UtcNow
        );

        _projectionMock
            .Setup(x => x.GetAsync(studentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(student);

        _projectionMock
            .Setup(x => x.GetAsync(friendId1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(friend1);
        _projectionMock
            .Setup(x => x.GetAsync(friendId2, It.IsAny<CancellationToken>()))
            .ReturnsAsync(friend2);

        var query = new ListFriendsByStudentIdQuery(studentId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(2);

        result.Value.Should().BeEquivalentTo(new[]
        {
            new ListFriendsByStudentIdResponse(friendId2, "Renan", 5, 1),
            new ListFriendsByStudentIdResponse(friendId1, "Augusto", 5, 3)
        }, options => options.WithStrictOrdering());
    }
}
