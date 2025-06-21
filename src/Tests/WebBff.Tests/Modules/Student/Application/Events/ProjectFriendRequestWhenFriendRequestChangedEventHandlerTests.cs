using Core.Shared.Errors;
using Core.Shared.Results;
using FluentAssertions;
using Moq;
using Serilog;
using Student.Application.UseCases.Events;
using Student.Domain;
using Student.Domain.Enumerations;
using Student.Persistence.Projections;
using Student.Shared.Commands;
using Xunit;
using MediatR;
using System.Linq.Expressions;

namespace WebBff.Tests.Modules.Student.Application.Events;

public class ProjectFriendRequestWhenFriendRequestChangedEventHandlerTests
{
    private readonly Mock<IStudentProjection<Projection.FriendRequests>> _friendRequestsProjectionMock = new();
    private readonly Mock<ISender> _senderMock = new();
    private readonly Mock<ILogger> _loggerMock = new();

    private readonly ProjectFriendRequestWhenFriendRequestChangedEventHandler _handler;

    public ProjectFriendRequestWhenFriendRequestChangedEventHandlerTests()
    {
        _handler = new ProjectFriendRequestWhenFriendRequestChangedEventHandler(
            _friendRequestsProjectionMock.Object,
            _loggerMock.Object,
            _senderMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Insert_FriendRequest_When_Created()
    {
        var @event = new DomainEvent.FriendRequestCreatedStatus(
            Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 1);

        _friendRequestsProjectionMock
            .Setup(x => x.ReplaceInsertAsync(It.IsAny<Projection.FriendRequests>(), It.IsAny<CancellationToken>()))
            .Returns(ValueTask.CompletedTask);

        await _handler.Handle(@event, CancellationToken.None);

        _friendRequestsProjectionMock.Verify(x => x.ReplaceInsertAsync(It.Is<Projection.FriendRequests>(r =>
            r.Id == @event.RequestId &&
            r.StudentId == @event.StudentId &&
            r.FriendId == @event.FriendId &&
            r.Status == FriendRequestStatus.Pending
        ), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Update_Status_When_Rejected()
    {
        var requestId = Guid.NewGuid();
        var @event = new DomainEvent.FriendRequestRejectedStatus(requestId, FriendRequestStatus.Rejected, 2);

        _friendRequestsProjectionMock
            .Setup(x => x.UpdateOneFieldAsync(
                requestId,
                It.IsAny<Expression<Func<Projection.FriendRequests, string>>>(),
                FriendRequestStatus.Rejected.ToString(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        await _handler.Handle(@event, CancellationToken.None);

        _friendRequestsProjectionMock.Verify(x => x.UpdateOneFieldAsync(
            requestId,
            It.IsAny<Expression<Func<Projection.FriendRequests, string>>>(),
            FriendRequestStatus.Rejected.ToString(),
            It.IsAny<CancellationToken>()), Times.Once);
    }
    [Fact]
    public async Task Handle_Should_Update_Status_And_Add_Friends_When_Accepted()
    {
        var studentId = Guid.NewGuid();
        var friendId = Guid.NewGuid();
        var requestId = Guid.NewGuid();

        var @event = new DomainEvent.FriendRequestAcceptedStatus(
            requestId, studentId, friendId, FriendRequestStatus.Accepted, 2);

        _friendRequestsProjectionMock
            .Setup(x => x.UpdateOneFieldAsync(
                requestId,
                It.IsAny<Expression<Func<Projection.FriendRequests, string>>>(),
                FriendRequestStatus.Accepted.ToString(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _senderMock
            .Setup(x => x.Send(It.Is<AddFriendCommand>(c => c.StudentId == studentId && c.FriendId == friendId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());

        _senderMock
            .Setup(x => x.Send(It.Is<AddFriendCommand>(c => c.StudentId == friendId && c.FriendId == studentId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());

        // Act
        await _handler.Handle(@event, CancellationToken.None);

        // Assert
        _friendRequestsProjectionMock.Verify(x => x.UpdateOneFieldAsync(
            requestId,
            It.IsAny<Expression<Func<Projection.FriendRequests, string>>>(),
            FriendRequestStatus.Accepted.ToString(),
            It.IsAny<CancellationToken>()), Times.Once);

        _senderMock.Verify(x => x.Send(It.Is<AddFriendCommand>(c =>
            c.StudentId == studentId && c.FriendId == friendId), It.IsAny<CancellationToken>()), Times.Once);

        _senderMock.Verify(x => x.Send(It.Is<AddFriendCommand>(c =>
            c.StudentId == friendId && c.FriendId == studentId), It.IsAny<CancellationToken>()), Times.Once);
    }


    [Fact]
    public async Task Handle_Should_Throw_When_AddFriend_Fails()
    {
        var studentId = Guid.NewGuid();
        var friendId = Guid.NewGuid();
        var requestId = Guid.NewGuid();

        var @event = new DomainEvent.FriendRequestAcceptedStatus(
            requestId, studentId, friendId, FriendRequestStatus.Accepted, 2);

        _friendRequestsProjectionMock
            .Setup(x => x.UpdateOneFieldAsync(
                requestId,
                It.IsAny<Expression<Func<Projection.FriendRequests, FriendRequestStatus>>>(),
                FriendRequestStatus.Accepted,
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _senderMock
            .Setup(x => x.Send(It.IsAny<AddFriendCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure(new Error("Fail", "Erro ao adicionar amigo")));

        Func<Task> act = async () => await _handler.Handle(@event, CancellationToken.None);

        await act.Should().ThrowAsync<Exception>().WithMessage($"Falha ao aceitar solicitação de amizade: {requestId}.");
    }
}

