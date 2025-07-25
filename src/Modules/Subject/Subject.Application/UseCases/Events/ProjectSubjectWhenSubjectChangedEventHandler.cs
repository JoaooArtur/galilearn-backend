﻿using Core.Application.EventBus;
using Serilog;
using Subject.Domain;
using Subject.Persistence.Projections;

namespace Subject.Application.UseCases.Events
{
    public interface IProjectSubjectWhenSubjectChangedEventHandler :
        IEventHandler<DomainEvent.SubjectCreated>,
        IEventHandler<DomainEvent.SubjectDeleted>;

    public class ProjectSubjectWhenSubjectChangedEventHandler(
        ISubjectProjection<Projection.Subject> subjectProjection,
        ILogger logger) : IProjectSubjectWhenSubjectChangedEventHandler
    {

        public async Task Handle(DomainEvent.SubjectCreated @event, CancellationToken cancellationToken = default)
        {
            try
            {
                await subjectProjection.ReplaceInsertAsync(new(
                    @event.SubjectId,
                    @event.Name,
                    @event.Description,
                    @event.Index,
                    @event.Timestamp),
                 cancellationToken);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Falha ao criar o assunto: {SubjectId}.", @event.SubjectId);

                var message = $"Falha ao criar o assunto: {@event.SubjectId}.";

                throw new InvalidOperationException(message, ex);
            }
        }
        public async Task Handle(DomainEvent.SubjectDeleted @event, CancellationToken cancellationToken = default)
        {
            try
            {
                await subjectProjection.DeleteAsync(x => x.Id == @event.SubjectId, cancellationToken);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Falha ao deletar o assunto: {SubjectId}.", @event.SubjectId);

                var message = $"Falha ao deletar o assunto: {@event.SubjectId}.";

                throw new InvalidOperationException(message, ex);
            }
        }
    }
}
