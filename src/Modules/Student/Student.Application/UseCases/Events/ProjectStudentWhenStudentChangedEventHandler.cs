using Core.Application.EventBus;
using Serilog;
using Student.Domain;
using Student.Persistence.Projections;

namespace Student.Application.UseCases.Events
{
    public interface IProjectStudentWhenStudentChangedEventHandler :
        IEventHandler<DomainEvent.StudentCreated>,
        IEventHandler<DomainEvent.StudentDeleted>,
        IEventHandler<DomainEvent.StudentActiveStatus>,
        IEventHandler<DomainEvent.StudentBlockedStatus>,
        IEventHandler<DomainEvent.FriendAdded>,
        IEventHandler<DomainEvent.StudentDefaultStatus>;

    public class ProjectStudentWhenStudentChangedEventHandler(
        IStudentProjection<Projection.Student> studentProjection,
        ILogger logger) : IProjectStudentWhenStudentChangedEventHandler
    {

        public async Task Handle(DomainEvent.StudentCreated @event, CancellationToken cancellationToken = default)
        {
            try
            {
                await studentProjection.ReplaceInsertAsync(new(
                    @event.StudentId,
                    @event.Name,
                    @event.Phone,
                    @event.Email,
                    @event.Status,
                    @event.DateOfBirth,
                    @event.Timestamp),
                 cancellationToken);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Falha ao criar o estudante: {@event.StudentId}.");

                throw;
            }
        }
        public async Task Handle(DomainEvent.StudentDeleted @event, CancellationToken cancellationToken = default)
        {
            try
            {
                await studentProjection.DeleteAsync(x => x.Id == @event.StudentId,
                 cancellationToken);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Falha ao deletar o estudante: {@event.StudentId}.");

                throw;
            }
        }

        public async Task Handle(DomainEvent.StudentActiveStatus @event, CancellationToken cancellationToken = default)
        {
            try
            {
                await studentProjection.UpdateOneFieldAsync(
                    id: @event.StudentId,
                    field: student => student.Status,
                    value: @event.Status,
                    cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Falha ao atualizar o status do estudante: {@event.StudentId}.");

                throw;
            }
        }

        public async Task Handle(DomainEvent.StudentBlockedStatus @event, CancellationToken cancellationToken = default)
        {
            try
            {
                await studentProjection.UpdateOneFieldAsync(
                    id: @event.StudentId,
                    field: student => student.Status,
                    value: @event.Status,
                    cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Falha ao atualizar o status do estudante: {@event.StudentId}.");

                throw;
            }
        }

        public async Task Handle(DomainEvent.StudentDefaultStatus @event, CancellationToken cancellationToken = default)
        {
            try
            {
                await studentProjection.UpdateOneFieldAsync(
                    id: @event.StudentId,
                    field: student => student.Status,
                    value: @event.Status,
                    cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Falha ao atualizar o status do estudante: {@event.StudentId}.");

                throw;
            }
        }
        public async Task Handle(DomainEvent.FriendAdded @event, CancellationToken cancellationToken = default)
        {
            try
            {
                //await studentProjection.UpdateOneFieldAsync(
                //    id: @event.StudentId,
                //    field: student => student.Status,
                //    value: @event.Status,
                //    cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Falha ao atualizar o status do estudante: {@event.StudentId}.");

                throw;
            }
        }
    }
}
