using Core.Application.EventBus;
using Serilog;
using Subject.Domain;
using Subject.Persistence.Projections;

namespace Subject.Application.UseCases.Events
{
    public interface IProjectLessonWhenLessonChangedEventHandler :
        IEventHandler<DomainEvent.LessonAdded>;

    public class ProjectLessonWhenLessonChangedEventHandler(
        ISubjectProjection<Projection.Lesson> lessonProjection,
        ILogger logger) : IProjectLessonWhenLessonChangedEventHandler
    {

        public async Task Handle(DomainEvent.LessonAdded @event, CancellationToken cancellationToken = default)
        {
            try
            {
                await lessonProjection.ReplaceInsertAsync(new(
                    @event.LessonId,
                    @event.SubjectId,
                    @event.Title,
                    @event.Content,
                    @event.Index,
                    @event.Timestamp),
                 cancellationToken);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Falha ao criar a aula: {@event.LessonId}.");

                throw;
            }
        }
    }
}
