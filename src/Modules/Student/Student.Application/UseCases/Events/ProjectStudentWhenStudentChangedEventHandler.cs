using Core.Application.EventBus;
using MediatR;
using MongoDB.Driver;
using Serilog;
using Student.Domain;
using Student.Persistence.Projections;
using Student.Shared.Commands;

namespace Student.Application.UseCases.Events
{
    public interface IProjectStudentWhenStudentChangedEventHandler :
        IEventHandler<DomainEvent.StudentCreated>,
        IEventHandler<DomainEvent.StudentDeleted>,
        IEventHandler<DomainEvent.StudentActiveStatus>,
        IEventHandler<DomainEvent.StudentBlockedStatus>,
        IEventHandler<DomainEvent.FriendAdded>,
        IEventHandler<DomainEvent.XpAdded>,
        IEventHandler<DomainEvent.StreakAdded>,
        IEventHandler<DomainEvent.StudentDefaultStatus>;

    public class ProjectStudentWhenStudentChangedEventHandler(
        IStudentProjection<Projection.Student> studentProjection,
        ISender sender,
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
                    1,
                    0,
                    50,
                    0,
                    [],
                    @event.DateOfBirth,
                    null,
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
                var collection = studentProjection.GetCollection();

                var question = await studentProjection.GetAsync(@event.StudentId, cancellationToken);

                var filter = Builders<Projection.Student>.Filter.Eq(x => x.Id, @event.StudentId);

                var pushUpdate = Builders<Projection.Student>.Update.Push(x => x.Friends, @event.Friend);

                await collection.UpdateOneAsync(filter, pushUpdate, new UpdateOptions { IsUpsert = true }, cancellationToken);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Falha ao adicionar amigo {@event.Friend} para o aluno: {@event.StudentId}.");

                throw;
            }
        }

        public async Task Handle(DomainEvent.XpAdded @event, CancellationToken cancellationToken = default)
        {
            try
            {
                var collection = studentProjection.GetCollection();

                var existingStudent = await studentProjection.GetAsync(@event.StudentId, cancellationToken);
                UpdateDefinition<Projection.Student> update;
                if (@event.LeveledUp)
                {
                    update = Builders<Projection.Student>.Update
                        .Set(customer => customer.Level, existingStudent.Level + 1)
                        .Set(customer => customer.Xp, (existingStudent.Xp + @event.XpAmount) - existingStudent.NextLevelXPNeeded)
                        .Set(customer => customer.NextLevelXPNeeded, 100 * (existingStudent.Level + 1));
                }
                else 
                {
                    update = Builders<Projection.Student>.Update
                        .Set(customer => customer.Xp, existingStudent.Xp + @event.XpAmount);
                }
                

                await collection.UpdateOneAsync(
                    filter: student => student.Id == @event.StudentId,
                    update: update,
                    cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Falha ao atualizar a experiência do estudante: {@event.StudentId}.");

                throw;
            }
        }
        public async Task Handle(DomainEvent.StreakAdded @event, CancellationToken cancellationToken = default)
        {
            try
            {
                var collection = studentProjection.GetCollection();

                var existingStudent = await studentProjection.GetAsync(@event.StudentId, cancellationToken);

                var update = Builders<Projection.Student>.Update
                    .Set(customer => customer.DaysStreak, existingStudent.DaysStreak + 1)
                    .Set(customer => customer.LastLessonAnswered, DateTimeOffset.Now);
                
                await collection.UpdateOneAsync(
                    filter: student => student.Id == @event.StudentId,
                    update: update,
                    cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Falha ao atualizar a sequência do estudante: {@event.StudentId}.");

                throw;
            }
        }
    }
}
