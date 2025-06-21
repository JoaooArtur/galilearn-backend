using Core.Application.EventBus;
using MediatR;
using Serilog;
using Student.Application.Services;
using Student.Domain;
using Student.Domain.Enumerations;
using Student.Persistence.Projections;
using Subject.Shared.Queries;

namespace Student.Application.UseCases.Events
{
    using StudentAggregate = Domain.Aggregates.Student;
    public interface IProjectLessonProgressWhenLessonProgressChangedEventHandler :
        IEventHandler<DomainEvent.LessonProgressCreated>,
        IEventHandler<DomainEvent.LessonProgressFinishedStatus>;

    public class ProjectLessonProgressWhenLessonProgressChangedEventHandler(
        IStudentProjection<Projection.LessonProgress> lessonProgressProjectionGateway,
        IStudentApplicationService applicationService,
        ISender sender,
        ILogger logger) : IProjectLessonProgressWhenLessonProgressChangedEventHandler
    {

        public async Task Handle(DomainEvent.LessonProgressCreated @event, CancellationToken cancellationToken = default)
        {
            try
            {
                await lessonProgressProjectionGateway.ReplaceInsertAsync(new(
                    @event.Id,
                    @event.SubjectId,
                    @event.LessonId,
                    @event.StudentId,
                    LessonStatus.InProgress,
                    @event.Timestamp),
                 cancellationToken);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Falha ao criar o progresso da lição: {LessonId}.", @event.LessonId);

                var message = $"Falha ao criar o progresso da lição: {@event.LessonId}.";

                throw new InvalidOperationException(message, ex);
            }
        }

        public async Task Handle(DomainEvent.LessonProgressFinishedStatus @event, CancellationToken cancellationToken = default)
        {
            try
            {
                var lesson = await lessonProgressProjectionGateway.FindAsync(x => x.SubjectId == @event.SubjectId && x.StudentId == @event.StudentId, cancellationToken);

                await lessonProgressProjectionGateway.UpdateOneFieldAsync(
                    id: lesson.Id,
                    field: attempt => attempt.Status,
                    value: @event.Status,
                    cancellationToken: cancellationToken);

                var finishedLessons = await lessonProgressProjectionGateway.ListAsync(x => x.SubjectId == @event.SubjectId && x.Status == LessonStatus.Finished, cancellationToken);

                var lessonsCountResult = await sender.Send(new GetLessonCountBySubjectIdQuery(@event.SubjectId), cancellationToken);

                if (lessonsCountResult.IsSuccess) 
                {
                    var lessonsCount = lessonsCountResult.Value;
                    if (finishedLessons.Count == lessonsCount.Count)
                    {
                        var studentResult = await applicationService.LoadAggregateAsync<StudentAggregate>(@event.StudentId, cancellationToken);

                        if (studentResult.IsFailure)
                            throw new Exception(studentResult.Error);
                        var student = studentResult.Value;

                        student.ChangeSubjectStatus(@event.SubjectId, SubjectStatus.Finished);

                        await applicationService.AppendEventsAsync(student, cancellationToken);

                        student.AddXp(50);

                        await applicationService.AppendEventsAsync(student, cancellationToken);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Falha ao atualizar o status da lição: {LessonId}.", @event.LessonId);

                var message = $"Falha ao atualizar o status da lição: {@event.LessonId}.";

                throw new InvalidOperationException(message, ex);
            }
        }
    }
}
