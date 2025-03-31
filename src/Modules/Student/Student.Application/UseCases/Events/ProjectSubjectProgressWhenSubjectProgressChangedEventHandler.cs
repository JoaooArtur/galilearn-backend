using Core.Application.EventBus;
using MediatR;
using Serilog;
using Student.Application.Services;
using Student.Domain;
using Student.Domain.Enumerations;
using Student.Persistence.Projections;

namespace Student.Application.UseCases.Events
{
    public interface IProjectSubjectProgressWhenSubjectProgressChangedEventHandler :
        IEventHandler<DomainEvent.SubjectProgressCreated>,
        IEventHandler<DomainEvent.SubjectProgressFinishedStatus>;

    public class ProjectSubjectProgressWhenSubjectProgressChangedEventHandler(
        IStudentProjection<Projection.SubjectProgress> subjectProgressProjectionGateway,
        IStudentApplicationService applicationService,
        ISender sender,
        ILogger logger) : IProjectSubjectProgressWhenSubjectProgressChangedEventHandler
    {

        public async Task Handle(DomainEvent.SubjectProgressCreated @event, CancellationToken cancellationToken = default)
        {
            try
            {
                await subjectProgressProjectionGateway.ReplaceInsertAsync(new(
                    @event.Id,
                    @event.SubjectId,
                    @event.StudentId,
                    LessonStatus.InProgress,
                    @event.Timestamp),
                 cancellationToken);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Falha ao criar o progresso do assunto: {@event.SubjectId}.");

                throw;
            }
        }

        public async Task Handle(DomainEvent.SubjectProgressFinishedStatus @event, CancellationToken cancellationToken = default)
        {
            try
            {
                var subject = await subjectProgressProjectionGateway.FindAsync(x => x.SubjectId == @event.SubjectId && x.StudentId == @event.StudentId, cancellationToken);

                await subjectProgressProjectionGateway.UpdateOneFieldAsync(
                    id: subject.Id,
                    field: attempt => attempt.Status,
                    value: @event.Status,
                    cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Falha ao atualizar o status do assunto: {@event.SubjectId}.");

                throw;
            }
        }
    }
}
