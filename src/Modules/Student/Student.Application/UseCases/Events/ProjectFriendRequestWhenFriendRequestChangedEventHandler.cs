using Core.Application.EventBus;
using MediatR;
using Serilog;
using Student.Domain;
using Student.Domain.Enumerations;
using Student.Persistence.Projections;
using Student.Shared.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Student.Application.UseCases.Events
{
    public interface IProjectFriendRequestWhenFriendRequestChangedEventHandler :
        IEventHandler<DomainEvent.FriendRequestCreatedStatus>,
        IEventHandler<DomainEvent.FriendRequestAcceptedStatus>,
        IEventHandler<DomainEvent.FriendRequestRejectedStatus>;

    public class ProjectFriendRequestWhenFriendRequestChangedEventHandler(
        IStudentProjection<Projection.FriendRequests> friendRequestsProjection,
        ILogger logger,
        ISender sender) : IProjectFriendRequestWhenFriendRequestChangedEventHandler
    {
        public async Task Handle(DomainEvent.FriendRequestCreatedStatus @event, CancellationToken cancellationToken = default)
        {
            try
            {
                await friendRequestsProjection.ReplaceInsertAsync(new(
                    @event.RequestId,
                    @event.StudentId,
                    @event.FriendId,
                    FriendRequestStatus.Pending,
                    @event.Timestamp),
                 cancellationToken);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Falha ao criar a solicitação de amizade: {RequestId}.", @event.RequestId);

                var message = $"Falha ao criar a solicitação de amizade: {@event.RequestId}.";

                throw new InvalidOperationException(message, ex);
            }
        }
        public async Task Handle(DomainEvent.FriendRequestRejectedStatus @event, CancellationToken cancellationToken = default)
        {
            try
            {
                await friendRequestsProjection.UpdateOneFieldAsync(
                    id: @event.RequestId,
                    field: student => student.Status,
                    value: @event.Status,
                    cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Falha ao rejeitar solicitação de amizade: {RequestId}.", @event.RequestId);

                var message = $"Falha ao rejeitar solicitação de amizade: {@event.RequestId}.";

                throw new InvalidOperationException(message, ex);
            }
        }

        public async Task Handle(DomainEvent.FriendRequestAcceptedStatus @event, CancellationToken cancellationToken = default)
        {
            try
            {
                await friendRequestsProjection.UpdateOneFieldAsync(
                    id: @event.RequestId,
                    field: student => student.Status,
                    value: @event.Status,
                    cancellationToken: cancellationToken);

                var studentFriendResult = await sender.Send(new AddFriendCommand(@event.StudentId, @event.FriendId), cancellationToken);

                if (studentFriendResult.IsFailure)
                    throw new InvalidOperationException(studentFriendResult.Error);

                var friendStudentResult = await sender.Send(new AddFriendCommand(StudentId: @event.FriendId, FriendId: @event.StudentId), cancellationToken);

                if (friendStudentResult.IsFailure)
                    throw new InvalidOperationException(friendStudentResult.Error);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Falha ao aceitar solicitação de amizade: {RequestId}.", @event.RequestId);

                var message = $"Falha ao aceitar solicitação de amizade: {@event.RequestId}.";

                throw new InvalidOperationException(message, ex);
            }
        }
    }
}
