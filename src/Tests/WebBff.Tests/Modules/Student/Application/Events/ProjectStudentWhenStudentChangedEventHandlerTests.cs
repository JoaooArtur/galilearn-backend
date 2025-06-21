using System.Linq.Expressions;
using Core.Shared.Results;
using FluentAssertions;
using Moq;
using MongoDB.Driver;
using Serilog;
using Student.Application.UseCases.Events;
using Student.Domain;
using Student.Domain.Enumerations;
using Student.Persistence.Projections;
using Xunit;
using MediatR;

namespace WebBff.Tests.Modules.Student.Application.Events;

public class ProjectStudentWhenStudentChangedEventHandlerTests
{
    private readonly Mock<IStudentProjection<Projection.Student>> _studentProjectionMock = new();
    private readonly Mock<ILogger> _loggerMock = new();

    private readonly ProjectStudentWhenStudentChangedEventHandler _handler;

    public ProjectStudentWhenStudentChangedEventHandlerTests()
    {
        _handler = new ProjectStudentWhenStudentChangedEventHandler(
            _studentProjectionMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Insert_Student_When_Created()
    {
        var @event = new DomainEvent.StudentCreated(Guid.NewGuid(), "teste@email.com", "password", "Teste", "11999999999", StudentStatus.Active, DateTimeOffset.UtcNow, DateTimeOffset.UtcNow, 1);

        _studentProjectionMock
            .Setup(x => x.ReplaceInsertAsync(It.IsAny<Projection.Student>(), It.IsAny<CancellationToken>()))
            .Returns(ValueTask.CompletedTask);

        await _handler.Handle(@event);

        _studentProjectionMock.Verify(x => x.ReplaceInsertAsync(It.Is<Projection.Student>(s =>
            s.Id == @event.StudentId &&
            s.Name == @event.Name &&
            s.Email == @event.Email &&
            s.Phone == @event.Phone &&
            s.Status == @event.Status), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Delete_Student_When_Deleted()
    {
        var studentId = Guid.NewGuid();
        var existingStudent = new Projection.Student(
            Id: studentId,
            Name: "Maria",
            Phone: "+5511988888888",
            Email: "teste@email.com",
            Status: "Active",
            Level: 1,
            Xp: 0,
            NextLevelXPNeeded: 100,
            DaysStreak: 0,
            Friends: [],
            DateOfBirth: DateTimeOffset.Parse("1999-05-05"),
            LastLessonAnswered: null,
            CreatedAt: DateTimeOffset.UtcNow
        );

        var @event = new DomainEvent.StudentDeleted(studentId, 2);

        _studentProjectionMock
            .Setup(x => x.DeleteAsync(It.IsAny<Expression<Func<Projection.Student, bool>>>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        await _handler.Handle(@event);

        _studentProjectionMock.Verify(x =>
            x.DeleteAsync(It.Is<Expression<Func<Projection.Student, bool>>>(expr =>
                expr.Compile().Invoke(existingStudent)),
                It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory]
    [InlineData("Active")]
    [InlineData("Blocked")]
    [InlineData("Default")]
    public async Task Handle_Should_Update_Status(string status)
    {
        var studentId = Guid.NewGuid();

        dynamic @event = status switch
        {
            "Active" => new DomainEvent.StudentActiveStatus(studentId, (StudentStatus)status, 2),
            "Blocked" => new DomainEvent.StudentBlockedStatus(studentId, (StudentStatus)status, 2),
            "Default" => new DomainEvent.StudentDefaultStatus(studentId, (StudentStatus)status, 2),
            _ => throw new InvalidOperationException()
        };

        _studentProjectionMock
            .Setup(x => x.UpdateOneFieldAsync(
                studentId,
                It.IsAny<Expression<Func<Projection.Student, string>>>(),
                status.ToString(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        await _handler.Handle(@event);

        _studentProjectionMock.Verify(x => x.UpdateOneFieldAsync(
            studentId,
            It.IsAny<Expression<Func<Projection.Student, string>>>(),
            status.ToString(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Add_Friend_When_FriendAdded()
    {
        var studentId = Guid.NewGuid();
        var friendId = Guid.NewGuid();

        var existingStudent = new Projection.Student(
            Id: studentId,
            Name: "Maria",
            Phone: "+5511988888888",
            Email: "teste@email.com",
            Status: "Active",
            Level: 1,
            Xp: 0,
            NextLevelXPNeeded: 100,
            DaysStreak: 0,
            Friends: [],
            DateOfBirth: DateTimeOffset.Parse("1999-05-05"),
            LastLessonAnswered: null,
            CreatedAt: DateTimeOffset.UtcNow
        );

        var @event = new DomainEvent.FriendAdded(studentId, friendId, 2);

        var collectionMock = new Mock<IMongoCollection<Projection.Student>>();
        _studentProjectionMock.Setup(x => x.GetCollection()).Returns(collectionMock.Object);
        _studentProjectionMock.Setup(x => x.GetAsync(studentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingStudent);

        collectionMock.Setup(x => x.UpdateOneAsync(
            It.IsAny<FilterDefinition<Projection.Student>>(),
            It.IsAny<UpdateDefinition<Projection.Student>>(),
            It.IsAny<UpdateOptions>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(Mock.Of<UpdateResult>());

        await _handler.Handle(@event);

        collectionMock.Verify(x => x.UpdateOneAsync(
            It.IsAny<FilterDefinition<Projection.Student>>(),
            It.IsAny<UpdateDefinition<Projection.Student>>(),
            It.IsAny<UpdateOptions>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task Handle_Should_Add_Xp_Correctly(bool leveledUp)
    {
        var studentId = Guid.NewGuid();
        var currentLevel = 1;
        var currentXp = 90;
        var nextXp = 100;
        var xpGained = 20;

        var existingStudent = new Projection.Student(
            Id: studentId,
            Name: "Maria",
            Phone: "+5511988888888",
            Email: "teste@email.com",
            Status: "Active",
            Level: currentLevel,
            Xp: currentXp,
            NextLevelXPNeeded: nextXp,
            DaysStreak: 0,
            Friends: [],
            DateOfBirth: DateTimeOffset.Parse("1999-05-05"),
            LastLessonAnswered: null,
            CreatedAt: DateTimeOffset.UtcNow
        );

        var @event = new DomainEvent.XpAdded(studentId, xpGained, leveledUp, 2);

        var collectionMock = new Mock<IMongoCollection<Projection.Student>>();
        _studentProjectionMock.Setup(x => x.GetCollection()).Returns(collectionMock.Object);
        _studentProjectionMock.Setup(x => x.GetAsync(studentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingStudent);

        collectionMock.Setup(x => x.UpdateOneAsync(
            It.IsAny<FilterDefinition<Projection.Student>>(),
            It.IsAny<UpdateDefinition<Projection.Student>>(),
            null,
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(Mock.Of<UpdateResult>());

        await _handler.Handle(@event);

        collectionMock.Verify(x => x.UpdateOneAsync(
            It.IsAny<FilterDefinition<Projection.Student>>(),
            It.IsAny<UpdateDefinition<Projection.Student>>(),
            null,
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Add_Streak()
    {
        var studentId = Guid.NewGuid();
        var streak = 3;

        var existingStudent = new Projection.Student(
            Id: studentId,
            Name: "Maria",
            Phone: "+5511988888888",
            Email: "teste@email.com",
            Status: "Active",
            Level: 1,
            Xp: 0,
            NextLevelXPNeeded: 100,
            DaysStreak: streak,
            Friends: [],
            DateOfBirth: DateTimeOffset.Parse("1999-05-05"),
            LastLessonAnswered: null,
            CreatedAt: DateTimeOffset.UtcNow
        );

        var @event = new DomainEvent.StreakAdded(studentId, 2);

        var collectionMock = new Mock<IMongoCollection<Projection.Student>>();
        _studentProjectionMock.Setup(x => x.GetCollection()).Returns(collectionMock.Object);
        _studentProjectionMock.Setup(x => x.GetAsync(studentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingStudent);

        collectionMock.Setup(x => x.UpdateOneAsync(
            It.IsAny<FilterDefinition<Projection.Student>>(),
            It.IsAny<UpdateDefinition<Projection.Student>>(),
            null,
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(Mock.Of<UpdateResult>());

        await _handler.Handle(@event);

        collectionMock.Verify(x => x.UpdateOneAsync(
            It.IsAny<FilterDefinition<Projection.Student>>(),
            It.IsAny<UpdateDefinition<Projection.Student>>(),
            null,
            It.IsAny<CancellationToken>()), Times.Once);
    }
}
